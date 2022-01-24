using NorthwindDataAccess.Dto.QueryDataDemo;
using NorthwindDataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthwindDataAccess.Dao
{
    public interface IQueryDemoDataDao
    {
        Task<List<Product>> FilterProductsAsync(string productName, string supplierCompanyName);
        Task<List<Product>> FilterProductsSpAsync(string productName, string supplierCompanyName);
        Task<int> GetProductCountAsync(string productName);
        Task<List<PagingResults>> PagedResultsDemoAsync(int page, int pageSize);
        Task<List<PagingResultsWithTotalCount>> PagedResultsWithCountAllDemoAsync(int page, int pageSize);
        Task<Dictionary<int, decimal>> SumOfUnitPricesPerSupplierUsingIterationAsync();
        Task<Dictionary<int, decimal>> SumOfUnitPricesPerSupplierWithoutIterationAsync();
        void WarmupOrms();
    }
}