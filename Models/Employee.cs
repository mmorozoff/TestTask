using System;

namespace TestTask.Models
{
    public class Employee
    {
        public int EmployeeID { set; get; }
        public string Surname { set; get; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Company { set; get; }
        public string Position { set; get; }
        public DateTime Date { get; set; }
    }
}
