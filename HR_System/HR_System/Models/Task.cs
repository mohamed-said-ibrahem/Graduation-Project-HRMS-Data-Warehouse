using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Task
    {
        private int taskId;
        private string taskName;
        private int projectId;
        private DateTime assignDate;
        private DateTime deadline;
        private List<Skill> skills=null;
        private List<EmployeeSkills> employeeSkills=null;
        private List<User_Info> employees = null;
        private DateTime endTime;
        public void scedualTask()
        {

        }

       

        public int TaskId { get => taskId; set => taskId = value; }
        public string TaskName { get => taskName; set => taskName = value; }
        public int ProjectId { get => projectId; set => projectId = value; }
        public DateTime AssignDate { get => assignDate; set => assignDate = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public List<Skill> Skills { get => skills; set => skills = value; }
        public List<EmployeeSkills> EmployeeSkills { get => employeeSkills; set => employeeSkills = value; }
        public List<User_Info> Employees { get => employees; set => employees = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }
    }
}