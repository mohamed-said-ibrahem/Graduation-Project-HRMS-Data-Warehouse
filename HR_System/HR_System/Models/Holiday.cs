using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Holiday
    {
        private string employeeName;
        private DateTime startDate;
        private DateTime endDate;
        private string cause;

        //public int EmployeeId { get => employeeId; set => employeeId = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public string Cause { get => cause; set => cause = value; }
        public string EmployeeName { get => employeeName; set => employeeName = value; }
    }
}