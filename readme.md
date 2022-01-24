**Linq2Db and EF Core Workshop app**

Sample application in .NET 5 which uses EF Core 5.0 to access database. Its an integral part of the Workshop related to Linq2Db.

**Requirements**
1. Visual Studio 2019 or newer with .NET Core development workloads installed
1. SQL Server instance with Northwind database available under **`Northwind`** name and accessible using Windows Authentication of the current user (this application is using hardcoded `Server=localhost;Database=Northwind;Trusted_Connection=True` connection string to connect to SQL Server database). A script named `northwind.sql` included in this repository can be used to create mentioned database. During the workshop a bigger backup of the database will be provided to better reflect performance differences.
1. SQL Server Profiler or other similer tool
1. SQL Server Management Studio
