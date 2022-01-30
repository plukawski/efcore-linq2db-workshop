using Linq2DbSynergyWorkshop.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NorthwindDataAccess.Dao;
using Polly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Linq2DbSynergyWorkshop
{
    public class WorhshopRunner
    {
        private readonly IQueryDemoDataDao queryDataDao;
        private readonly IModifyDemoDataDao modifyDataDao;
        private readonly IServiceProvider container;

        public WorhshopRunner(
            IQueryDemoDataDao queryDao,
            IModifyDemoDataDao modifyDao,
            IServiceProvider container)
        {
            this.queryDataDao = queryDao;
            this.modifyDataDao = modifyDao;
            this.container = container;
            this.queryDataDao.WarmupOrms();
        }

        public async Task ProblematicQuery_Excercise1()
        {
            Console.WriteLine("-------Problematic Query-------");
            var results = await queryDataDao.PagedResultsDemoAsync(0, 5);
            var results2 = await queryDataDao.PagedResultsDemoAsync(1, 5);
            foreach (var result in results)
            {
                Console.WriteLine($"{result.ProductName} - {result.UnitPrice}");
            }
            Console.WriteLine("--------");
            foreach (var result in results2)
            {
                Console.WriteLine($"{result.ProductName} - {result.UnitPrice}");
            }
            Console.WriteLine("-------Problematic Query-------");
        }

        public async Task UpdateLotOfRecords_Excercise2()
        {
            Console.WriteLine("-------Batch UPDATE-------");
            using (Stopper stopper = new Stopper("EF Core update"))
            {
                await modifyDataDao.UpdateEmployeesEfCoreAsync();
            }
            using (Stopper stopper = new Stopper("Linq2db update"))
            {
                await modifyDataDao.UpdateEmployeesLinq2DbAsync();
            }
            Console.WriteLine("-------Batch UPDATE-------");
        }

        public async Task InsertLotOfRecords_Excercise3()
        {
            Console.WriteLine("-------Batch INSERT-------");
            using (Stopper stopper = new Stopper("EF Core insertion"))
            {
                await modifyDataDao.InsertLotOfRecordsEfCoreAsync();
            }

            using (Stopper stopper = new Stopper("Linq2Db batch insertion"))
            {
                await modifyDataDao.InsertLotOfRecordsLinq2DbAsync();
            }
            Console.WriteLine("-------Batch INSERT-------");
        }

        public async Task Upsert_Excercise4()
        {
            Console.WriteLine("-------UPSERT-------");
            IQueryDemoDataDao queryDataDao = container.GetService<IQueryDemoDataDao>();
            Random random = new Random();
            string productName = $"upsert_{random.Next()}";
            string linq2DbProductName = $"upsert_linq2db_{random.Next()}";
            string wrongProductName = $"wrongupsert_{random.Next()}";

            var tasks = new List<Task>();
            var retryPolisy = Policy.Handle<Exception>().RetryAsync(3);
            tasks = new List<Task>();
            using (Stopper stopper = new Stopper("Wrong Upsert"))
            {
                for (int i = 0; i < 10; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using var containerScope = container.CreateScope();
                        IModifyDemoDataDao modifyDataDao = containerScope.ServiceProvider.GetService<IModifyDemoDataDao>();
                        await retryPolisy.ExecuteAsync(async () =>
                        {
                            await modifyDataDao.WrongUpsertProductDemoAsync(wrongProductName);
                        });
                    }));
                }

                try
                {
                    await Task.WhenAll(tasks.ToArray());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wrong Upsert - Errors occured during execution:{ex.Message}");
                }
            }

            tasks = new List<Task>();
            using (Stopper stopper = new Stopper("Upsert"))
            {
                for (int i = 0; i < 10; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using var containerScope = container.CreateScope();
                        IModifyDemoDataDao modifyDataDao = containerScope.ServiceProvider.GetService<IModifyDemoDataDao>();
                        await retryPolisy.ExecuteAsync(async () =>
                        {
                            await modifyDataDao.UpsertProductSpDemoAsync(productName);
                        });
                    }));
                }

                try
                {
                    await Task.WhenAll(tasks.ToArray());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Upsert - Errors occured during execution:{ex.Message}");
                }
            }

            tasks = new List<Task>();
            using (Stopper stopper = new Stopper("Linq2db Upsert"))
            {
                for (int i = 0; i < 10; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        using var containerScope = container.CreateScope();
                        IModifyDemoDataDao modifyDataDao = containerScope.ServiceProvider.GetService<IModifyDemoDataDao>();
                        await retryPolisy.ExecuteAsync(async () =>
                        {
                            await modifyDataDao.UpsertProductDemoLinq2DbAsync(linq2DbProductName);
                        });
                    }));
                }

                try
                {
                    await Task.WhenAll(tasks.ToArray());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Linq2Db Upsert - Errors occured during execution:{ex.Message}");
                }
            }

            int wrongProductsCount = await queryDataDao.GetProductCountAsync(wrongProductName);
            int productsCount = await queryDataDao.GetProductCountAsync(productName);
            int linq2DbProductsCount = await queryDataDao.GetProductCountAsync(linq2DbProductName);
            Console.WriteLine($"Wrong Upsert - number of products in db: {wrongProductsCount}");
            Console.WriteLine($"Upsert - number of products in db: {productsCount}");
            Console.WriteLine($"Linq2Db Upsert - number of products in db: {linq2DbProductsCount}");

            Console.WriteLine("-------UPSERT-------");
        }

        public async Task OptionalParameters_Excercise5()
        {
            Console.WriteLine("-------Optional parameters-------");
            await queryDataDao.FilterProductsAsync(null, "test");    //warmup
            await queryDataDao.FilterProductsSpAsync(null, "test");    //warmup

            using (Stopper stopper = new Stopper("EF Core filter products"))
            {
                await queryDataDao.FilterProductsAsync(null, "exotic");
            }
            
            using (Stopper stopper = new Stopper("SP filter products"))
            {
                await queryDataDao.FilterProductsSpAsync(null, "exotic");
            }
            Console.WriteLine("-------Optional parameters-------");
        }

        public async Task AnalyticFunctions_Excercise6()
        {
            Console.WriteLine("-------Analytic functions-------");

            var results = await queryDataDao.PagedResultsWithCountAllDemoAsync(0, 5);
            foreach (var result in results)
            {
                Console.WriteLine($"{result.ProductName} - {result.UnitPrice} - {result.MinUnitPricePerSupplier} - {result.TotalCount}");
            }

            Console.WriteLine("-------Analytic functions-------");
        }

        public async Task IterationsAsLethalSins_Excercise7()
        {
            await queryDataDao.SumOfUnitPricesPerSupplierWithoutIterationAsync();   //warmup
            await queryDataDao.SumOfUnitPricesPerSupplierUsingIterationAsync(); //warmup
            Console.WriteLine("-------Iterations in databases-------");
            using (Stopper stopper = new Stopper("No iterations used"))
            {
                var results = await queryDataDao.SumOfUnitPricesPerSupplierWithoutIterationAsync();
            }

            using (Stopper stopper = new Stopper("Iterations used"))
            {
                var result = await queryDataDao.SumOfUnitPricesPerSupplierUsingIterationAsync();
            }
            Console.WriteLine("-------Iterations in databases-------");
        }
    }
}
