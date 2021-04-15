using System;

namespace TestTask.ViewModel
{
    public class AddEmployee
    {
        public int EmployeeID { set; get; }
        public string Surname { set; get; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Company { set; get; }
        public string Position { set; get; }
        public DateTime Date { get; set; }
        public string[] Companies { set; get; }
    }
}
