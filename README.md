# BlogPostCategory.API

## Pre-Requsites
1. Visual Studio
2. SQL Server Management Studio (SSMS)

Once you have cloned this repository, open the **CodePulse.API sln** using **Visual Studio**.

Open MSSQL and create a new database.

In the solution, open the **appsettings.json** and change the *Server* and *Database* based on the Server and Database created in your MSSQL.

In the **Package Manager Console**, run `Update-Database` to restore the migration.
