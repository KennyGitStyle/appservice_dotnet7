using AutoMapper;
using Mapping.Entities;
using MappingObjects.Mappers;

Cart cart = new(
    Customer: new(
        FirstName: "Kenneth",
        LastName: "Hendricks"),
    Items: new()
    {
        new(ProductName: "Pear", UnitPrice: 0.49M, Quantity: 10),
        new(ProductName: "Orange", UnitPrice: 1.50M, Quantity: 12)
    });
WriteLine($"{cart.Customer}");
foreach (LineItem item in cart.Items)
{
    WriteLine($"{item}");
}

MapperConfiguration config = CartToSummaryMapper.GetMappingConfiguration();
IMapper mapper = config.CreateMapper();
Summary summary = mapper.Map<Cart, Summary>(cart);
WriteLine($"Summary: {summary.FullName} spent {summary.Total}.");