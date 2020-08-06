using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Applicant
    {
        private int applicantId;
        private string name;
        private string email;
        private string number;
        private string ssn;
        private string address;
        private string education;
        private string graduationYear;
        private string yearsOfExperience;
        private string gendre;
        private List<Skill> skills;

        public int ApplicantId { get => applicantId; set => applicantId = value; }
        public string Name { get => name; set => name = value; }
        public string Number { get => number; set => number = value; }
        public string Email { get => email; set => email = value; }
        public string Ssn { get => ssn; set => ssn = value; }
        public string Address { get => address; set => address = value; }
        public string Education { get => education; set => education = value; }
        public string GraduationYear { get => graduationYear; set => graduationYear = value; }
        public string YearsOfExperience { get => yearsOfExperience; set => yearsOfExperience = value; }
        public string Gendre { get => gendre; set => gendre = value; }
        public List<Skill> Skills { get => skills; set => skills = value; }
    }
}