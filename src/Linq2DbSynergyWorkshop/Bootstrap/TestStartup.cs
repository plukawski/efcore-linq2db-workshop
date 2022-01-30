using Linq2DbSynergyWorkshop.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwindDataAccess;

namespace Linq2DbSynergyWorkshop.Bootstrap
{
    class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
            this.sqliteConnection = new SqliteConnection("DataSource=NorthwindInMemory;Mode=Memory;Cache=Shared");
            sqliteConnection.Open();
        }

        public SqliteConnection sqliteConnection { get; private set; }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            var testDbOptions = new DbContextOptionsBuilder<NorthwindContext>()
                .UseSqlite("DataSource=NorthwindInMemory;Mode=Memory;Cache=Shared")
                .Options;

            using (var context = new NorthwindContext(testDbOptions))
            {
                context.Database.EnsureCreated();
                TestDataHelper.PrepareTestData(context);
            }

            services.AddScoped(ctx => new NorthwindContext(testDbOptions));
        }
    }
}
