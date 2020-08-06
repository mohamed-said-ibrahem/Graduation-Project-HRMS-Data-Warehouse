using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Bonus
    {
        private int bonusId;
        private int departmentId;
        private int positionId;
        private int minSkillsRank;
        private double minPerformancePercentage;
        private double minAttendancePercentage;
        private double bonusValue;
        private string description;


        public int BonusId { get => bonusId; set => bonusId = value; }
        public int DepartmentId { get => departmentId; set => departmentId = value; }
        public int PositionId { get => positionId; set => positionId = value; }
        public int MinSkillsRank { get => minSkillsRank; set => minSkillsRank = value; }
        public double MinPerformancePercentage { get => minPerformancePercentage; set => minPerformancePercentage = value; }
        public double BonusValue { get => bonusValue; set => bonusValue = value; }
        public double MinAttendancePercentage { get => minAttendancePercentage; set => minAttendancePercentage = value; }
        public string Description { get => description; set => description = value; }
    }
}