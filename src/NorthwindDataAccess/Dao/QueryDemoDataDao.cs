using Microsoft.EntityFrameworkCore;
using NorthwindDataAccess.Dto.QueryDataDemo;
using NorthwindDataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindDataAccess.Dao
{
    public class QueryDemoDataDao : IQueryDemoDataDao
    {
        private readonly NorthwindContext context;

        public QueryDemoDataDao(NorthwindContext context)
        {
            this.context = context;
        }

        public async Task<List<PagingResults>> PagedResultsDemoAsync(int page, int pageSize)
        {
            var query1 = from p in context.Products
                         join s in context.Suppliers
                            on p.SupplierId equals s.SupplierId
                         where s.CompanyName == "New Orleans Cajun Delights" 
                            || s.CompanyName == "Grandma Kelly's Homestead"
                         select new
                         {
                             UnitPrice = p.UnitPrice ?? 0,
                             p.ProductName
                         };

            return await query1
                .OrderBy(c => c.ProductName)
                .Distinct()
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(x => new PagingResults()
                {
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice
                })
                .ToListAsync();
        }

        /*
         EF Core:
        exec sp_executesql N'SELECT [t].[ProductName], [t].[c] AS [UnitPrice]
        FROM (
            SELECT DISTINCT [p].[ProductID], COALESCE([p].[UnitPrice], 0.0) AS [c], [p].[ProductName]
            FROM [Products] AS [p]
            INNER JOIN [Suppliers] AS [s] ON [p].[SupplierID] = [s].[SupplierID]
            WHERE [s].[CompanyName] IN (N''New Orleans Cajun Delights'', N''Grandma Kelly''''s Homestead'')
        ) AS [t]
        ORDER BY (SELECT 1) --WTF is THAT?
        OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY',N'@__p_0 int,@__p_1 int',@__p_0=0,@__p_1=5
        */

        public async Task<List<Product>> FilterProductsAsync(string productName, string supplierCompanyName)
        {
            var query = from p in context.Products
                            .Where(x => x.ProductName.StartsWith(productName) 
                                || x.Supplier.CompanyName.StartsWith(supplierCompanyName))
                        select p;

            return await query
                .ToListAsync();
        }

        public async Task<List<Product>> FilterProductsSpAsync(string productName, string supplierCompanyName)
        {
            return await context.Products
                .FromSqlRaw("EXEC [dbo].[demo_FilterProducts] {0}, {1}", productName, supplierCompanyName)
                .ToListAsync();
        }
        /*
         CREATE PROCEDURE [dbo].[demo_FilterProducts]
	        -- Add the parameters for the stored procedure here
	        @productName NVARCHAR(40),
	        @supplierCompanyName NVARCHAR(40)
        AS
        BEGIN
	        -- SET NOCOUNT ON added to prevent extra result sets from
	        -- interfering with SELECT statements.
	        SET NOCOUNT ON;

            SELECT p.*
	        FROM Products p
	        LEFT JOIN Suppliers s on s.SupplierID = p.SupplierID
	        WHERE 1=1
	        AND (@productName IS NULL OR p.ProductName LIKE @productName+N'%')
	        AND (@supplierCompanyName IS NULL OR s.CompanyName LIKE @supplierCompanyName+N'%')
	
        END
         */
        
        public async Task<List<PagingResultsWithTotalCount>> PagedResultsWithCountAllDemoAsync(int page, int pageSize)
        {
            string sql = @"
            SELECT 
		        p.ProductName, 
		        p.UnitPrice,
		        COUNT(*) OVER() AS TotalCount,
                MIN(UnitPrice) OVER (PARTITION BY p.SupplierID) AS MinUnitPricePerSupplier
	        From Products p
	        WHERE p.ReorderLevel > 20
            ORDER BY p.ProductName
            OFFSET @p0 ROWS FETCH NEXT @p1 ROWS ONLY";

            return await context.PaginResultsWithTotalCount
                .FromSqlRaw(sql, page, pageSize)
                .ToListAsync();
        }

        public async Task<Dictionary<int, decimal>> SumOfUnitPricesPerSupplierUsingIterationAsync()
        {
            Dictionary<int, decimal> results = new Dictionary<int, decimal>();
            var supplierIds = await context.Suppliers
                .Where(x => x.SupplierId < 4000)
                .Select(x => x.SupplierId)
                .ToListAsync();
            foreach (var supplierId in supplierIds)
            {
                var sumOfUnitPrices = await context.Products
                    .Where(x => x.SupplierId == supplierId)
                    .SumAsync(x => x.UnitPrice ?? 0);
                results.Add(supplierId, sumOfUnitPrices);
            }
            return results;
        }

        public async Task<Dictionary<int, decimal>> SumOfUnitPricesPerSupplierWithoutIterationAsync()
        {
            Dictionary<int, decimal> results = new Dictionary<int, decimal>();
            var query = from s in context.Suppliers
                        join p in context.Products on s.SupplierId equals p.SupplierId into pj
                        from p in pj.DefaultIfEmpty()
                        where s.SupplierId < 4000
                        group p by s.SupplierId into grp
                        select new
                        {
                            SupplierId = grp.Key,
                            UnitPrices = grp.Sum(x => x.UnitPrice ?? 0)
                        };

            return await query.ToDictionaryAsync(x => x.SupplierId, y => y.UnitPrices);
        }

        public async Task<int> GetProductCountAsync(string productName)
        {
            return await context.Products.CountAsync(x => x.ProductName == productName);
        }

        public void WarmupOrms()
        {
            context.Products.Count();
        }
    }
}
