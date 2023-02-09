using Northwind.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.HttpLogging;
using AspNetCoreRateLimit;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

bool useMicrosoftRateLimiting = true;



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNorthwindContext();

if (useMicrosoftRateLimiting)
{
    builder.Services.AddMemoryCache();
    builder.Services.AddInMemoryRateLimiting();
    builder.Services.Configure<ClientRateLimitOptions>(
        builder.Configuration.GetSection("ClientRateLimiting"));
    builder.Services.Configure<ClientRateLimitPolicies>(
        builder.Configuration.GetSection("ClientRateLimitPolicies"));
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
}

builder.Services.AddHttpLogging(options =>
{
    options.RequestHeaders.Add("Origin");
    options.RequestHeaders.Add("X-Client-Id");
    options.ResponseHeaders.Add("Retry-After");
    options.LoggingFields = HttpLoggingFields.All;
});

string northwindMvc = "Northwind.Mvc.Policy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: northwindMvc,
        policy =>
        {
            policy.WithOrigins("https://localhost:5092");
        });
});

var app = builder.Build();

if(useMicrosoftRateLimiting)
{
    RateLimiterOptions rateLimiterOptions = new();
    rateLimiterOptions.AddFixedWindowLimiter(
        policyName: "fixed5per10seconds", options =>
        {
            options.PermitLimit = 5;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 2;
            options.Window = TimeSpan.FromSeconds(10);
        });
    app.UseRateLimiter(rateLimiterOptions);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpLogging();
//app.UseCors(policyName: northwindMvc);
app.UseCors();

app.MapGet("/", () => "Hello World!")
    .ExcludeFromDescription();
int pageSize = 10;

app.MapGet("api/products", (
  [FromServices] NorthwindContext db,
  [FromQuery] int? page) =>
  db.Products.Where(product =>
    (product.UnitsInStock > 0) && (!product.Discontinued))
    .Skip(((page ?? 1) - 1) * pageSize).Take(pageSize)
  )
  .WithName("GetProducts")
  .WithOpenApi(operation =>
  {
      operation.Description =
        "Get products with UnitsInStock > 0 and Discontinued = false.";
      operation.Summary = "Get in-stock products that are not discontinued.";
      return operation;
  })
  .Produces<Product[]>(StatusCodes.Status200OK)
  .RequireRateLimiting("fixed5per10seconds");

app.MapGet("api/products/outofstock", ([FromServices] NorthwindContext db) =>
db.Products.Where(product =>
(product.UnitsInStock == 0) && (!product.Discontinued)))
    .WithName("GetProductsOutOfStock")
    .WithOpenApi()
    .Produces<Product[]>(StatusCodes.Status200OK);

app.MapGet("api/products/{id:int}",
    async Task<Results<Ok<Product>, NotFound>> ([FromServices] NorthwindContext db,
    [FromRoute] int id) =>
    await db.Products.FindAsync(id) is Product product ?
    TypedResults.Ok(product) : TypedResults.NotFound())
    .WithName("GetProductById")
    .WithOpenApi()
    .Produces<Product>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .RequireCors(policyName:northwindMvc);

app.MapGet("api/products/{name}", ([FromServices] NorthwindContext db, [FromRoute] string name)
    => db.Products.Where(p => p.ProductName.Contains(name)))
    .WithName("GetProductByName")
    .WithOpenApi()
    .Produces<Product[]>(StatusCodes.Status200OK)
    .RequireCors(policyName: northwindMvc);

app.MapPost("api/products", async ([FromBody] Product product, [FromServices] NorthwindContext db) =>
{
    await db.Products.AddAsync(product);
    await db.SaveChangesAsync();
    return Results.Created($"api/products/{product.ProductId}", product);
}).WithOpenApi()
.Produces<Product>(StatusCodes.Status201Created);

app.MapPut("api/products/{id:int}", async (
    [FromRoute] int id,
    [FromBody] Product product,
    [FromServices] NorthwindContext db) =>
{
    Product? foundProduct = await db.Products.FindAsync(id);
    if (foundProduct is null)
        return Results.NotFound();
    foundProduct.ProductName = product.ProductName;
    foundProduct.CategoryId = product.CategoryId;
    foundProduct.SupplierId = product.SupplierId;
    foundProduct.QuantityPerUnit = product.QuantityPerUnit;
    foundProduct.UnitsInStock = product.UnitsInStock;
    foundProduct.UnitsOnOrder = product.UnitsOnOrder;
    foundProduct.ReorderLevel = product.ReorderLevel;
    foundProduct.UnitPrice = product.UnitPrice;
    foundProduct.Discontinued = product.Discontinued;
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithOpenApi()
.Produces(StatusCodes.Status404NotFound)
.Produces<Product>(StatusCodes.Status204NoContent);

app.MapDelete("api/products/{id:int}", async (
  [FromRoute] int id,
  [FromServices] NorthwindContext db) =>
{
    if (await db.Products.FindAsync(id) is Product product)
    {
        db.Products.Remove(product);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
}).WithOpenApi()
  .Produces(StatusCodes.Status404NotFound)
  .Produces(StatusCodes.Status204NoContent);
app.Run();