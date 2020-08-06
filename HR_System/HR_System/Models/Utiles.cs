using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Utiles
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["HRCon"].ToString();
            con = new SqlConnection(constring);
        }
        public string getProjectName(int projectId)
        {
            string projectName = "";
            connection();
            SqlCommand cmd = new SqlCommand("get_project_info", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@project_id", projectId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                projectName = Convert.ToString(dr["project_name"]);
            }
            return projectName;
        }

        public List<Skill> skill_list()
        {
            List<Skill> skill_list = new List<Skill>();
            connection();
            SqlCommand cmd = new SqlCommand("skill_list", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Skill skill = new Skill();
                skill.SkillId = Convert.ToInt32(dr["skill_id"]);
                skill.SkillName = Convert.ToString(dr["skill_name"]);
                skill_list.Add(skill);
            }

            return skill_list;

        }
        public List<Applicant> applicants_list()
        {
            List<Applicant> applicant_list = new List<Applicant>();
            connection();
            SqlCommand cmd = new SqlCommand("applicant_list", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Applicant applicant = new Applicant(); 
                applicant.ApplicantId = Convert.ToInt32(dr["applicant_id"]);
                applicant.Name = Convert.ToString(dr["name"]);
                applicant_list.Add(applicant);
            }

            return applicant_list;

        }

        public List<Project> project_list()
        {
            List<Project> projects= new List<Project>();

            connection();
            SqlCommand cmd = new SqlCommand("project_list", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Project project = new Project();
                project.ProjectId = Convert.ToInt32(dr["project_id"]);
                project.ProjectName = Convert.ToString(dr["project_name"]);
                projects.Add(project);
            }

            return projects;

        }
        internal List<Client> client_list()
        {
            List<Client> clients = new List<Client>();
            connection();
            SqlCommand cmd = new SqlCommand("client_list", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Client client = new Client();
                client.ClientId = Convert.ToInt32(dr["client_id"]);
                client.Name = Convert.ToString(dr["client_name"]);
                clients.Add(client);
            }

            return clients;
        }

        public string  skill_name(int skill_id)
        {
            string name = "";
            connection();
            SqlCommand cmd = new SqlCommand("skill_name", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@skill_id", skill_id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
               
                name = Convert.ToString(dr["skill_name"]);
                
            }

            return name;

        }
        public string department_name(int department_id)
        {
            string name = "";
            connection();
            SqlCommand cmd = new SqlCommand("department_name", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@department_id", department_id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {

                name = Convert.ToString(dr["department_name"]);

            }

            return name;

        }
        public Position position_info(int position_id)
        {
            Position position = new Position();
            connection();
            SqlCommand cmd = new SqlCommand("position_info", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@position_id", position_id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {

                position.PositionName = Convert.ToString(dr["position_name"]);
               

            }

            return position;

        }
        public List<Department> department_list()
        {
            List<Department> department_list = new List<Department>();
            connection();
            SqlCommand cmd = new SqlCommand("department_list", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Department department = new Department();
                department.DepartmentId = Convert.ToInt32(dr["department_id"]);
                department.Name = Convert.ToString(dr["department_name"]);
                department_list.Add(department);
            }

            return department_list;
        }

        public List<Position> position_list()
        {
            List<Position> position_list = new List<Position>();
            connection();
            SqlCommand cmd = new SqlCommand("position_list", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Position position = new Position();
                position.PositionId = Convert.ToInt32(dr["position_id"]);
                position.PositionName = Convert.ToString(dr["position_name"]);
                position_list.Add(position);
            }

            return position_list;
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public List<User_Info> GetEmployeesInfo(List<int> employees_id)
        {
            List<User_Info> employees = new List<User_Info>();
            for(int i=0; i < employees_id.Count; i++)
            {
                User_Info user = new User_Info();

                connection();
                SqlCommand cmd = new SqlCommand("employee_info", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", employees_id[i]);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    user.User_id = Convert.ToInt32(dr["employee_id"]);
                    user.UserName = Convert.ToString(dr["user_name"]);
                    user.Email = Convert.ToString(dr["email"]);
                    user.PhoneNumber = Convert.ToString(dr["phone_number"]);
                    user.Address = Convert.ToString(dr["address"]);
                }
                employees.Add(user);

            }
            

            return employees;
        }

        public int getIdByUserName(string userName)
        {
            int id = 0;

            connection();
            SqlCommand cmd = new SqlCommand("getIdByUserName", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userName", userName);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {

                id= Convert.ToInt32(dr["employee_id"]);
                break;

            }

            return id;
        }

        public int task_id(string name)
        {
            int task_id = 0;
            connection();
            SqlCommand cmd = new SqlCommand("task_id", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", name);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {

                task_id = Convert.ToInt32(dr["task_id"]);

            }

            return task_id;
        }
        public int getTaskId(string projectName, string taskName)
        {
            int taskId = 0;
            connection();
            SqlCommand cmd = new SqlCommand("get_task_id", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@project_name", projectName);
            cmd.Parameters.AddWithValue("@task_name", taskName);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                taskId = Convert.ToInt32(dr["task_id"]);
            }
            return taskId;

        }
    }
}