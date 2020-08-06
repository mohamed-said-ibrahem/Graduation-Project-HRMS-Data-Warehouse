using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Project
    {
        private int projectId;
        private string projectName;
        private Department projectDepartment ;
        private Client client;
        private double projectCost;
		private int teamleaderID;
        private DateTime startDate;
        private DateTime endDate;
        private DateTime actualEndTime;
        private double performance;
        private List<Task> task;

        public int ProjectId { get => projectId; set => projectId = value; }
        public string ProjectName { get => projectName; set => projectName = value; }
        public Department ProjectDepartment { get => projectDepartment; set => projectDepartment = value; }
        public Client Client { get => client; set => client = value; }
        public double ProjectCost { get => projectCost; set => projectCost = value; }
        public int TeamleaderID { get => teamleaderID; set => teamleaderID = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public double Performance { get => performance; set => performance = value; }
        public List<Task> Task { get => task; set => task = value; }
        public DateTime ActualEndTime { get => actualEndTime; set => actualEndTime = value; }
    }
}