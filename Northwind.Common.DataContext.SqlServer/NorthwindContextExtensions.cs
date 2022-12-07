using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Shared
{
    public static class NorthwindContextExtensions
    {
        /// <summary>
        /// Adds NorthwindContext to the specified IServiceCollection. Use the SqlServer database provider.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">Set to override the default.</param>
        /// <returns>An IServicesCollection thst can be to add more services.</returns>
        public static IServiceCollection AddNorthwindContext(this IServiceCollection services, connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Northwind;" +
            "Integrated Security=true;MultipleActiveResultsets=true;Encrypt=false")
        {
  
            return services.AddDbContext<NorthwindContext>(opts =>
            {
                opts.UseSqlServer(connString);
                opts.LogTo(WriteLine,
                    new[] { Microsoft.EntityFrameworkCore.Diagnostic.RelationalEventId.CommandExecuting });
            });
        }
    }
}
