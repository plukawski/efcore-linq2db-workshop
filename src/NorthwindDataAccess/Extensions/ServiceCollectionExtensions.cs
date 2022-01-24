using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwindDataAccess.Dao;

namespace NorthwindDataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNorthwindDemoDataAccess(this IServiceCollection services, string connectionString)
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                    .UseSqlServer(connectionString)
                    .Options;

            services.AddScoped<NorthwindContext>((ctx) => 
                new NorthwindContext(options, ctx.GetService<NorthwindCommandInterceptor>()));
            services.AddSingleton<NorthwindCommandInterceptor>();

            services.AddTransient<IQueryDemoDataDao, QueryDemoDataDao>();
            services.AddTransient<IModifyDemoDataDao, ModifyDemoDataDao>();
        }
    }
}
