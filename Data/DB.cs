using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Data
{
    public class DB
    {
        public static string connectionString { set; get; } = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
        public static string[] getEmployeeTable { set; get; } = {"SELECT * FROM Employee ORDER BY EmployeeID ASC",
                                                                 "SELECT * FROM Employee ORDER BY Surname ASC",
                                                                 "SELECT * FROM Employee ORDER BY Company ASC" };
        public static string[] columnNamesEmployee { set; get; } = { "EmployeeID", "Surname", "FirstName", "Patronymic", "Date", "Position", "Company" };
        public static string companyList { set; get; } = "SELECT CompanyName FROM Company";
        public static string insertEmployee { set; get; } = "INSERT INTO Employee (Surname, FirstName, Patronymic, Date, Position, Company) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";
        public static string findEmployee { set; get; } = "SELECT * FROM Employee WHERE EmployeeID = '{0}'";
        public static string deleteEmployee { set; get; } = "DELETE FROM Employee WHERE EmployeeID = '{0}'";
        public static string deleteEmployeeByCompany { set; get; } = "DELETE FROM Employee WHERE Company = '{0}'";
        public static string updateEmployee { set; get; } = "UPDATE Employee SET Surname = '{0}', FirstName = '{1}', Patronymic = '{2}', Date = '{3}', Position = '{4}', Company = '{5}' WHERE EmployeeID = '{6}'";
        //-----------------------Company--------------------------
        public static string[] getCompanyTable { set; get; } = {"SELECT * FROM Company ORDER BY CompanyID ASC",
                                                                "SELECT * FROM Company ORDER BY CompanyName ASC",
                                                                "SELECT * FROM Company ORDER BY CompanySize ASC" };
        public static string[] columnNamesCompany { set; get; } = { "CompanyID", "CompanyName", "Organization", "CompanySize" };
        public static string insertCompany { set; get; } = "INSERT INTO Company (CompanyName, Organization, CompanySize) VALUES ('{0}', '{1}', '{2}')";
        public static string findCompany { set; get; } = "SELECT * FROM Company WHERE CompanyID = '{0}'";
        public static string deleteCompany { set; get; } = "DELETE FROM Company WHERE CompanyName = '{0}'";
        public static string updateCompany { set; get; } = "UPDATE Company SET CompanyName = '{0}', Organization = '{1}', CompanySize = '{2}' WHERE CompanyID = '{3}'";

    }
}
