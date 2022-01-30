using Linq2DbSynergyWorkshop.Bootstrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Linq2DbSynergyWorkshop
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Startup startup = new Startup(ParseParameters(args));
            TestStartup testStartup = new TestStartup(ParseParameters(args));
            IServiceCollection services = new ServiceCollection();
            testStartup.ConfigureServices(services);
            using var mainContainer = services.BuildServiceProvider();
            using (var mainContainerScope = mainContainer.CreateScope())
            {
                var runner = mainContainerScope.ServiceProvider.GetService<WorhshopRunner>();

                await runner.ProblematicQuery_Excercise1();
                //await runner.UpdateLotOfRecords_Excercise2();
                //await runner.InsertLotOfRecords_Excercise3();
                //await runner.Upsert_Excercise4();
                //await runner.OptionalParameters_Excercise5();
                //await runner.AnalyticFunctions_Excercise6();
                //await runner.IterationsAsLethalSins_Excercise7();
            }

            testStartup.sqliteConnection?.Close();
            Console.WriteLine("Excercise ended!");
            Console.ReadKey();
        }

        private static IConfiguration ParseParameters(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddCommandLine(args);
            return configurationBuilder.Build();
        }
    }
}
