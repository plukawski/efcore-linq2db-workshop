namespace NorthwindDataAccess.Dto.QueryDataDemo
{
    public class PagingResultsWithTotalCount : PagingResults
    {
        public int TotalCount { get; set; }
        public decimal MinUnitPricePerSupplier { get; set; }
    }
}
