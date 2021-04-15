using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.Models;
using TestTask.ViewModel;

namespace TestTask.Controllers
{
    public class UserController : Controller
    {

        //---------------------------------Employee------------------------------------
        public IActionResult Employees(string state = "id")  //show all employees in database
        {
            Employee[] employees = new Employee[0];
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
            string sqlExpression = "SELECT * FROM Employee ORDER BY EmployeeID ASC";
            if (state == "surname")
            {
                sqlExpression = "SELECT * FROM Employee ORDER BY Surname ASC";
            }
            if (state == "company")
            {
                sqlExpression = "SELECT * FROM Employee ORDER BY Company ASC";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Employee Temp = new Employee
                        {
                            EmployeeID = (int)reader.GetValue(0),
                            Surname = (string)reader.GetValue(1),
                            FirstName = (string)reader.GetValue(2),
                            Patronymic = (string)reader.GetValue(3),
                            Date = (DateTime)reader.GetValue(4),
                            Position = (string)reader.GetValue(5),
                            Company = (string)reader.GetValue(6)
                        };
                        Array.Resize(ref employees, employees.Length + 1);
                        employees[^1] = Temp;
                    }
                }
                reader.Close();
            }
            return View(employees);
        }

        [HttpGet]
        public IActionResult AddEmployee() //add new employee to database 
        {
            AddEmployee employee = new AddEmployee();
            string[] result = new string[0];
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
            string sqlExpression = "SELECT CompanyName FROM Company";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Array.Resize(ref result, result.Length + 1);
                        result[^1] = (string)reader.GetValue(0);
                    }
                }
                reader.Close();
            }
            employee.Companies = result;
            employee.Date = DateTime.Now;
            return View(employee);
        }

        [HttpPost]
        public IActionResult AddEmployee(AddEmployee model, string state) //add new employee in database (post method)
        {
            if (ModelState.IsValid)
            {
                if (state != "Отмена")
                {
                    string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
                    string sqlExpression = "INSERT INTO Employee (Surname, FirstName, Patronymic, Date, Position, Company) VALUES (@Surname, @FirstName, @Patronymic, @Date, @Position, @Company)";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.Parameters.AddWithValue("Surname", model.Surname);
                        command.Parameters.AddWithValue("@FirstName", model.FirstName);
                        command.Parameters.AddWithValue("@Patronymic", model.Patronymic);
                        command.Parameters.AddWithValue("@Date", model.Date);
                        command.Parameters.AddWithValue("@Position", model.Position);
                        command.Parameters.AddWithValue("@Company", model.Company);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Employees", "User");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditEmployee(int id) //editing selected employee
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
            string sqlExpression = string.Format("SELECT * FROM Employee WHERE EmployeeID = '{0}'", id);
            AddEmployee employee = new AddEmployee();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee.EmployeeID = (int)reader.GetValue(0);
                        employee.Surname = (string)reader.GetValue(1);
                        employee.FirstName = (string)reader.GetValue(2);
                        employee.Patronymic = (string)reader.GetValue(3);
                        employee.Date = (DateTime)reader.GetValue(4);
                        employee.Position = (string)reader.GetValue(5);
                        employee.Company = (string)reader.GetValue(6);
                    }
                }

                reader.Close();
            }
            string[] result = new string[0];
            connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
            sqlExpression = "SELECT CompanyName FROM Company";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Array.Resize(ref result, result.Length + 1);
                        result[^1] = (string)reader.GetValue(0);
                    }
                }
                reader.Close();
            }
            employee.Companies = result;
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEmployee(AddEmployee model, int id) //editing selected employee (post method)
        {
            if (ModelState.IsValid)
            {
                string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
                string sqlExpression = string.Format("UPDATE Employee SET Surname = '{0}', FirstName = '{1}', Patronymic = '{2}', Date = '{3}', Position = '{4}', Company = '{5}' WHERE EmployeeID = '{6}'",
                    model.Surname, model.FirstName, model.Patronymic, model.Date, model.Position, model.Company, id);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                }
                return RedirectToAction("Employees", "User");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Action(string[] AreChecked, IFormCollection form) //deleting employees with using checkboxes
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
            foreach (var item in AreChecked)
            {
                if (form.Keys.Contains("delete"))
                {
                    string sqlExpression = string.Format("DELETE FROM Employee WHERE EmployeeID = '{0}'", item);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        int number = command.ExecuteNonQuery();
                    }
                }

            }
            return RedirectToAction("Employees", "User");
        }

        //------------------------------Company--------------------------------------

        public IActionResult Companies(string state = "id")
        {
            Company[] companies = new Company[0];
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
            string sqlExpression = "SELECT * FROM Company ORDER BY CompanyID ASC";
            if (state == "name")
            {
                sqlExpression = "SELECT * FROM Company ORDER BY CompanyName ASC";
            }
            if (state == "size")
            {
                sqlExpression = "SELECT * FROM Company ORDER BY CompanySize ASC";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.CompanyID = (int)reader.GetValue(0);
                        company.CompanyName = (string)reader.GetValue(1);
                        company.Organization = (string)reader.GetValue(2);
                        company.Size = (int)reader.GetValue(3);
                        Array.Resize(ref companies, companies.Length + 1);
                        companies[^1] = company;
                    }
                }
                reader.Close();
            }
            return View(companies);
        }

        [HttpGet]
        public IActionResult AddCompany()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCompany(AddCompany model, string state)
        {
            if (ModelState.IsValid)
            {
                if (state != "Отмена")
                {
                    string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
                    string sqlExpression = "INSERT INTO Company (CompanyName, Organization, CompanySize) VALUES (@CompanyName, @Organization, @Size)";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                        command.Parameters.AddWithValue("@Organization", model.Organization);
                        command.Parameters.AddWithValue("@Size", model.Size);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Companies", "User");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditCompany(int id)
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
            string sqlExpression = string.Format("SELECT * FROM Company WHERE CompanyID = '{0}'", id);
            AddCompany company = new AddCompany();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        company.CompanyID = (int)reader.GetValue(0);
                        company.CompanyName = (string)reader.GetValue(1);
                        company.Organization = (string)reader.GetValue(2);
                        company.Size = (int)reader.GetValue(3);
                    }
                }

                reader.Close();
            }
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCompany(AddCompany model, int id)
        {
            if (ModelState.IsValid)
            {
                string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
                string sqlExpression = string.Format("UPDATE Company SET CompanyName = '{0}', Organization = '{1}', CompanySize = '{2}' WHERE CompanyID = '{3}'",
                    model.CompanyName, model.Organization, model.Size, id);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                }
                return RedirectToAction("Companies", "User");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ActionCompany(string[] AreChecked, IFormCollection form)
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=testdb;Integrated Security=True";
            foreach (var item in AreChecked)
            {
                if (form.Keys.Contains("delete"))
                {
                    string sqlExpression = string.Format("DELETE FROM Employee WHERE Company = '{0}'", item);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        int number = command.ExecuteNonQuery();
                    }
                    sqlExpression = string.Format("DELETE FROM Company WHERE CompanyName = '{0}'", item);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        int number = command.ExecuteNonQuery();
                    }
                }

            }
            return RedirectToAction("Companies", "User");
        }
    }
}
