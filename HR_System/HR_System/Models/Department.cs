using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Department
    {
        private int departmentId;
        private string name;
        private int managerId;

        public int DepartmentId { get => departmentId; set => departmentId = value; }
        public string Name { get => name; set => name = value; }
        public int ManagerId { get => managerId; set => managerId = value; }
    }
}