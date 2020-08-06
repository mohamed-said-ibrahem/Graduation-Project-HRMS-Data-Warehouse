using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Skill
    {
        private int skillId;
        private string skillName;
        private int rate;
        private string skillDescription;

        public int SkillId { get => skillId; set => skillId = value; }
        public string SkillName { get => skillName; set => skillName = value; }
        public int Rate { get => rate; set => rate = value; }
        public string SkillDescription { get => skillDescription; set => skillDescription = value; }
    }
}