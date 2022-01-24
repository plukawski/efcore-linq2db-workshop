using NorthwindDataAccess;

namespace Linq2DbSynergyWorkshop.Helpers
{
    public class TestDataHelper
    {
        public static void PrepareTestData(NorthwindContext context)
        {
            context.Categories.Add(new NorthwindDataAccess.Entities.Category()
            {
                CategoryName = "Test category",
                Description = ""
            });

            context.Suppliers.Add(new NorthwindDataAccess.Entities.Supplier()
            {
                CompanyName = "Test company",
                ContactName = "Test contact",
            });

            var supplierHavingProducts = new NorthwindDataAccess.Entities.Supplier()
            {
                CompanyName = "New Orleans Cajun Delights",
                ContactName = "Test contact 2",
            };
            context.Suppliers.Add(supplierHavingProducts);

            for (int i = 15; i > 0; i--)
            {
                context.Products.Add(new NorthwindDataAccess.Entities.Product()
                {
                    Supplier = supplierHavingProducts,
                    ProductName = $"Test product {i}",
                    UnitPrice = i,
                    UnitsInStock = (short)i
                });
            }

            context.SaveChanges();
        }
    }
}
