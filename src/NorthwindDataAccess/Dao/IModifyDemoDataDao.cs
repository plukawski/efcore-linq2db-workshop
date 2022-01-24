using System.Threading.Tasks;

namespace NorthwindDataAccess.Dao
{
    public interface IModifyDemoDataDao
    {
        Task InsertLotOfRecordsEfCoreAsync();
        Task UpdateEmployeesEfCoreAsync();
        Task UpsertProductSpDemoAsync(string productName);
        Task WrongUpsertProductDemoAsync(string productName);
    }
}