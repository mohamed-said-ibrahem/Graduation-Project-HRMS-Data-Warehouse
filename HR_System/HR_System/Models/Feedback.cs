using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Feedback
    {
        private int employeeGiverId;
        private int employeeReceiverId;
        private int taskId;
        private int skillId;
        private int rank;
        private string description;

        public int EmployeeGiverId { get => employeeGiverId; set => employeeGiverId = value; }
        public int EmployeeReceiverId { get => employeeReceiverId; set => employeeReceiverId = value; }
        public int TaskId { get => taskId; set => taskId = value; }
        public int SkillId { get => skillId; set => skillId = value; }
        public int Rank { get => rank; set => rank = value; }
        public string Description { get => description; set => description = value; }
    }
}