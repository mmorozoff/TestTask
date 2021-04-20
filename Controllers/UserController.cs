using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.Models;
using TestTask.ViewModel;
using TestTask.Data;
using System.Threading.Tasks;

namespace TestTask.Controllers
{
    public class UserController : Controller
    {
        string[] buttonStates = { "Отмена", "delete" };
        //---------------------------------Employee------------------------------------
        public IActionResult Employees(int state = 0)  //show all employees in database
        {
            Employee[] employees = new Employee[0];
            string sqlExpression = DB.getEmployeeTable[state];
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                try
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
                                EmployeeID = (int)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[0])),
                                Surname = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[1])),
                                FirstName = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[2])),
                                Patronymic = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[3])),
                                Date = (DateTime)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[4])),
                                Position = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[5])),
                                Company = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[6]))
                            };
                            Array.Resize(ref employees, employees.Length + 1);
                            employees[^1] = Temp;
                        }
                    }
                    reader.Close();
                }
                catch (SqlException err)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            return View(employees);
        }

        [HttpGet]
        public IActionResult AddEmployee() //add new employee to database 
        {
            AddEmployee employee = new AddEmployee();
            string[] result = new string[0];
            string sqlExpression = DB.companyList;
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Array.Resize(ref result, result.Length + 1);
                            result[^1] = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesCompany[1]));
                        }
                    }
                    reader.Close();
                }
                catch (SqlException err)
                {
                    //throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            employee.Companies = result;
            return View(employee);
        }

        [HttpPost]
        public IActionResult AddEmployee(AddEmployee model, string state) //add new employee in database (post method)
        {
            if (ModelState.IsValid)
            {
                if (state != buttonStates[0])
                {
                    string sqlExpression = string.Format(DB.insertEmployee, model.Surname, model.FirstName, model.Patronymic, model.Date, model.Position, model.Company);
                    using (SqlConnection connection = new SqlConnection(DB.connectionString))
                    {
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlExpression, connection);
                            int number = command.ExecuteNonQuery();
                        }
                        catch (SqlException err)
                        {
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                return RedirectToAction("Employees", "User");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditEmployee(int id) //editing selected employee
        {
            string sqlExpression = string.Format(DB.findEmployee, id);
            AddEmployee employee = new AddEmployee();
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            employee.EmployeeID = (int)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[0]));
                            employee.Surname = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[1]));
                            employee.FirstName = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[2]));
                            employee.Patronymic = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[3]));
                            employee.Date = (DateTime)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[4]));
                            employee.Position = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[5]));
                            employee.Company = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesEmployee[6]));
                        }
                    }

                    reader.Close();
                }
                catch (SqlException err)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            string[] result = new string[0];
            sqlExpression = DB.companyList;
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Array.Resize(ref result, result.Length + 1);
                            result[^1] = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesCompany[1]));
                        }
                    }
                    reader.Close();
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
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
                string sqlExpression = string.Format(DB.updateEmployee, model.Surname, model.FirstName, model.Patronymic, model.Date, model.Position, model.Company, id);
                using (SqlConnection connection = new SqlConnection(DB.connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException err)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return RedirectToAction("Employees", "User");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Action(string[] AreChecked, IFormCollection form) //deleting employees with using checkboxes
        {
            foreach (var item in AreChecked)
            {
                if (form.Keys.Contains(buttonStates[1]))
                {
                    string sqlExpression = string.Format(DB.deleteEmployee, item);
                    using (SqlConnection connection = new SqlConnection(DB.connectionString))
                    {
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlExpression, connection);
                            int number = command.ExecuteNonQuery();
                        }
                        catch (SqlException err)
                        {
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }

            }
            return RedirectToAction("Employees", "User");
        }

        //------------------------------Company--------------------------------------

        public IActionResult Companies(int state = 0)
        {
            Company[] companies = new Company[0];
            string sqlExpression = DB.getCompanyTable[state];
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Company company = new Company();
                            company.CompanyID = (int)reader.GetValue(reader.GetOrdinal(DB.columnNamesCompany[0]));
                            company.CompanyName = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesCompany[1]));
                            company.Organization = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesCompany[2]));
                            company.Size = (int)reader.GetValue(reader.GetOrdinal(DB.columnNamesCompany[3]));
                            Array.Resize(ref companies, companies.Length + 1);
                            companies[^1] = company;
                        }
                    }
                    reader.Close();
                }
                catch (SqlException err)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
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
                if (state != buttonStates[0])
                {
                    string sqlExpression = string.Format(DB.insertCompany, model.CompanyName, model.Organization, model.Size);
                    using (SqlConnection connection = new SqlConnection(DB.connectionString))
                    {
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlExpression, connection);
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException err)
                        {
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                return RedirectToAction("Companies", "User");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditCompany(int id)
        {
            string sqlExpression = string.Format(DB.findCompany, id);
            AddCompany company = new AddCompany();
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            company.CompanyID = (int)reader.GetValue(reader.GetOrdinal(DB.columnNamesCompany[0]));
                            company.CompanyName = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesCompany[1]));
                            company.Organization = (string)reader.GetValue(reader.GetOrdinal(DB.columnNamesCompany[2]));
                            company.Size = (int)reader.GetValue(reader.GetOrdinal(DB.columnNamesCompany[3]));
                        }
                    }

                    reader.Close();
                }
                catch (SqlException err)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCompany(AddCompany model, int id)
        {
            if (ModelState.IsValid)
            {
                string sqlExpression = string.Format(DB.updateCompany, model.CompanyName, model.Organization, model.Size, id);
                using (SqlConnection connection = new SqlConnection(DB.connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException err)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return RedirectToAction("Companies", "User");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ActionCompany(string[] AreChecked, IFormCollection form)
        {
            foreach (var item in AreChecked)
            {
                if (form.Keys.Contains(buttonStates[1]))
                {
                    string sqlExpression = string.Format(DB.deleteEmployeeByCompany, item);
                    using (SqlConnection connection = new SqlConnection(DB.connectionString))
                    {
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlExpression, connection);
                            int number = command.ExecuteNonQuery();
                        }
                        catch (SqlException err)
                        {
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    sqlExpression = string.Format(DB.deleteCompany, item);
                    using (SqlConnection connection = new SqlConnection(DB.connectionString))
                    {
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlExpression, connection);
                            int number = command.ExecuteNonQuery();
                        }
                        catch (SqlException err)
                        {
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }

            }
            return RedirectToAction("Companies", "User");
        }
    }
}
