using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Permission
    {
        private string employeeName;
        private DateTime startTime;
        private DateTime endTime;
        private string cause;

       // public int EmployeeId { get => employeeId; set => employeeId = value; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }
        public string Cause { get => cause; set => cause = value; }
        public string EmployeeName { get => employeeName; set => employeeName = value; }
    }
}