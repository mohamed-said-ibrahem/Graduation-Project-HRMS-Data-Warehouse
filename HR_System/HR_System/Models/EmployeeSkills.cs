using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class EmployeeSkills
    {
        private int employeeId;
        private int skillId;

        
        public int EmployeeId { get => employeeId; set => employeeId = value; }
        public int SkillId { get => skillId; set => skillId = value; }
    }
}