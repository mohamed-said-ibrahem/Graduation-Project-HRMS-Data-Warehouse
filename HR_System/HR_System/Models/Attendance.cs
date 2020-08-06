using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Attendance
    {
        private int employeeId;
        private TimeSpan arrivalTime;
        private TimeSpan leaveTime;
        private DateTime date;
        private string employeeName;
        private string note;

        public int EmployeeId { get => employeeId; set => employeeId = value; }
        public TimeSpan ArrivalTime { get => arrivalTime; set => arrivalTime = value; }
        public TimeSpan LeaveTime { get => leaveTime; set => leaveTime = value; }
        public DateTime Date { get => date; set => date = value; }
        public string EmployeeName { get => employeeName; set => employeeName = value; }
        public string Note { get => note; set => note = value; }
    }
}