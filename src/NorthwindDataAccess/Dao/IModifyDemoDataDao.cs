using System.Threading.Tasks;

namespace NorthwindDataAccess.Dao
{
    public interface IModifyDemoDataDao
    {
        Task InsertLotOfRecordsEfCoreAsync();
        Task InsertLotOfRecordsLinq2DbAsync();
        Task UpdateEmployeesEfCoreAsync();
        Task UpdateEmployeesLinq2DbAsync();
        Task UpsertProductDemoLinq2DbAsync(string productName);
        Task UpsertProductSpDemoAsync(string productName);
        Task WrongUpsertProductDemoAsync(string productName);
    }
}