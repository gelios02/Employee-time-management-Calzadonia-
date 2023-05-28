using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;
using Firebase.Database.Query;

namespace Calzadonia_1111111.Employee
{
    public class Employee 
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsSelected { get; set; }
        public Dictionary<string, string> Schedule { get; set; }
    }
    public class WorkDay
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    }

    public class Schedule
    {
        public WorkDay Monday { get; set; }
        public WorkDay Tuesday { get; set; }
        public WorkDay Wednesday { get; set; }
        public WorkDay Thursday { get; set; }
        public WorkDay Friday { get; set; }
        public WorkDay Saturday { get; set; }
        public WorkDay Sunday { get; set; }
    }
}
