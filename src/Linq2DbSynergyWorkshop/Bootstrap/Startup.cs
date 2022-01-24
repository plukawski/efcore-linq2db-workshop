using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NorthwindDataAccess.Extensions;

namespace Linq2DbSynergyWorkshop.Bootstrap
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(logger => 
            {
                logger.AddConsole();
            });

            services.AddNorthwindDemoDataAccess("Server=localhost;Database=Northwind;Trusted_Connection=True");
            services.AddScoped<WorhshopRunner>();
        }
    }
}
