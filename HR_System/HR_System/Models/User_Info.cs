using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class User_Info
    {
        private int user_id;
        private string userName;
        private string password;
        private Department department;
        private Position position;
        private string fullName;
        private string email;
        private Double salary;
        private string phoneNumber;
        private string ssn;
        private string address;
        private DateTime startDate;
        private string gender;
        private DateTime birthDate;
        private string educationalDrgree;
        private DateTime graduationDate;
        private string notes;
        private List<Skill> employeeSkills;
        public int User_id { get => user_id; set => user_id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
       
        public string FullName { get => fullName; set => fullName = value; }
        public string Email { get => email; set => email = value; }
        public double Salary { get => salary; set => salary = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Ssn { get => ssn; set => ssn = value; }
        public string Address { get => address; set => address = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public string Gender { get => gender; set => gender = value; }
        public DateTime BirthDate { get => birthDate; set => birthDate = value; }
        public string EducationalDrgree { get => educationalDrgree; set => educationalDrgree = value; }
        public DateTime GraduationDate { get => graduationDate; set => graduationDate = value; }
        public string Notes { get => notes; set => notes = value; }
        public List<Skill> EmployeeSkills { get => employeeSkills; set => employeeSkills = value; }
        public Department Department { get => department; set => department = value; }
        public Position Position { get => position; set => position = value; }
    }
}