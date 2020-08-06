using HR_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class TeamLeader:Employee
    {
        private int departmentId;
        private Project project;
        private SqlConnection con;

        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["HRCon"].ToString();
            con = new SqlConnection(constring);
        }
        public int createProject(Project project)
        {
            int projectId = 0;
            //string message;
            connection();
            SqlCommand cmd = new SqlCommand("createProject", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@project_name", project.ProjectName);
            cmd.Parameters.AddWithValue("@teamleader_id", project.TeamleaderID);
            cmd.Parameters.AddWithValue("@department_id", project.ProjectDepartment.DepartmentId);
            cmd.Parameters.AddWithValue("@client_id", project.Client.ClientId);
            cmd.Parameters.AddWithValue("@start_date", project.StartDate);
            cmd.Parameters.AddWithValue("@end_date", project.EndDate);
            cmd.Parameters.AddWithValue("@price", project.ProjectCost);
            cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
           // cmd.Parameters.Add("@message", SqlDbType.VarChar).Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            projectId = Convert.ToInt32(cmd.Parameters["@id"].Value);
           // message = Convert.ToString(cmd.Parameters["@message"].Value);
            con.Close();
            return projectId;

        }
        public int createTask(Task task)
        {

            int taskId = 0;
            /// inset tasks into task table
            connection();
            SqlCommand cmd = new SqlCommand("createTask", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@task_name", task.TaskName);
            cmd.Parameters.AddWithValue("@project_id", task.ProjectId);
            cmd.Parameters.AddWithValue("@assign_time", task.AssignDate);
            cmd.Parameters.AddWithValue("@deadline", task.Deadline);
            cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            taskId = Convert.ToInt32(cmd.Parameters["@id"].Value);
            con.Close();

            return taskId;

        }
        //get list of employees with skills

        public List<User_Info> getRecommendedEmployeeForTask(Task task)
        {
            List<Skill> skills = task.Skills;
            List<User_Info> employees = new List<User_Info>();
            for (int i = 0; i < skills.Count; i++)
            {
                connection();
                SqlCommand cmd = new SqlCommand("get_recommended_employee_for_skill", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@skill_id", skills[i].SkillId);
                cmd.Parameters.AddWithValue("@start_date", task.AssignDate);
                cmd.Parameters.AddWithValue("@end_date", task.Deadline);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                sd.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    User_Info user = new User_Info();
                    user.User_id = Convert.ToInt32(dr["employee_id"]);
                    user.UserName = Convert.ToString(dr["user_name"]);
                    user.Email = Convert.ToString(dr["email"]);
                    user.PhoneNumber = Convert.ToString(dr["phone_number"]);
                    user.Address = Convert.ToString(dr["address"]);
                    Skill skill = new Skill();
                    skill.SkillId = skills[i].SkillId;
                    skill.SkillName = Convert.ToString(dr["skill_name"]);

                    skill.Rate = Convert.ToInt32(dr["skill_rank"]);
                    user.EmployeeSkills = new List<Skill>();
                    user.EmployeeSkills.Add(skill);
                    employees.Add(user);
                }
            }
            return employees;
        }


        // input is the list of tasks with employees and skill employees
        public int AssignEmployeeForTask(int task_id,int employee_id,DateTime assignDate,DateTime deadline)
        {
            // insert tasks inro employee task

            connection();
            SqlCommand cmd = new SqlCommand("isEmployeeAvailableOnDate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeId", employee_id);
            string start_date = assignDate.Date.ToString("MM/dd/yyyy");
            cmd.Parameters.AddWithValue("@start_date", start_date);
            string end_date = deadline.Date.ToString("MM/dd/yyyy");
            cmd.Parameters.AddWithValue("@end_date", end_date);
            cmd.Parameters.Add("@count", SqlDbType.Int).Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            int count = 0;
            count = Convert.ToInt32(cmd.Parameters["@count"].Value);
            if (count > 0)
            {
                // employee is not available at the training time
                return 0;
            }
            // employee available 
            else
            {
                //assign 

                connection();
                SqlCommand cmd_1 = new SqlCommand("insert_task_employees", con);
                cmd_1.CommandType = CommandType.StoredProcedure;
                cmd_1.Parameters.AddWithValue("@task_id", task_id);
                cmd_1.Parameters.AddWithValue("@employee_id", employee_id);

                con.Open();
                cmd_1.ExecuteNonQuery();
                con.Close();

                return 1;

            }
            

        }

        public void setEmployeeTask(int employeeId, int task_id, List<int> skillId)
        {
            for (int i = 0; i < skillId.Count; i++)
            {
                connection();
                SqlCommand cmd = new SqlCommand("insert_task_skills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@task_id", task_id);
                cmd.Parameters.AddWithValue("@skill_id", skillId[i]);
                cmd.Parameters.AddWithValue("@employee_id", employeeId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
        }

        public void addTaskEndTime(int taskId)
        {
            connection();
            SqlCommand cmd = new SqlCommand("set_end_time_for_task", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@task_id", taskId);
            cmd.Parameters.AddWithValue("@end_time", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public List<Project> viewAllProject(int teamLeaderId) {
            List<Project> projects = new List<Project>();
            connection();
            SqlCommand cmd = new SqlCommand("get_all_projects_teamLeaderView", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@teamLeaderId", teamLeaderId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Project project = new Project();
                project.ProjectDepartment = new Department();
                project.Client = new Client();
                project.ProjectName = Convert.ToString(dr["project_name"]);
                project.ProjectId = Convert.ToInt32(dr["project_id"]);
                project.Client.ClientId = Convert.ToInt32(dr["client_id"]);
                project.Client.Name = Convert.ToString(dr["client_name"]);
                project.ProjectCost = Convert.ToDouble(dr["price"]);
                project.TeamleaderID = Convert.ToInt32(dr["teamleader_id"]);
                project.ProjectDepartment.Name = Convert.ToString(dr["department_name"]);
                project.StartDate = Convert.ToDateTime(dr["start_date"]);
                project.EndDate = Convert.ToDateTime(dr["end_date"]);
                if (!DBNull.Value.Equals(dr["actual_end_date"]))
                {
                    project.ActualEndTime = Convert.ToDateTime(dr["actual_end_date"]).Date;

                }
                else
                {
                    project.ActualEndTime = DateTime.MinValue.Date;
                }
                projects.Add(project);

            }

            return projects;
        }



        public Task ViewTask(int taskId)
        {
            Task task = new Task();
            connection();
            SqlCommand cmd = new SqlCommand("get_task", con);///////////////////
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@task_id", taskId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                task.TaskId = Convert.ToInt32(dr["task_id"]);
                task.TaskName = Convert.ToString(dr["task_name"]);
                task.ProjectId = Convert.ToInt32(dr["project_id"]);
                task.AssignDate = Convert.ToDateTime(dr["assign_time"]);
                task.Deadline = Convert.ToDateTime(dr["deadline"]);
            }
            List<User_Info> employees = new List<User_Info>();
            connection();
            cmd = new SqlCommand("get_task_employees", con);///////////////
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@task_id", taskId);
            sd = new SqlDataAdapter(cmd);
            dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                User_Info user = new User_Info();
                user.User_id = Convert.ToInt32(dr["employee_id"]);
                user.UserName = Convert.ToString(dr["user_name"]);
                user.Email = Convert.ToString(dr["email"]);
                user.PhoneNumber = Convert.ToString(dr["phone_number"]);
                user.Address = Convert.ToString(dr["address"]);
                if (!DBNull.Value.Equals(dr["end_time"]))
                {
                    task.EndTime = Convert.ToDateTime(dr["end_time"]);
                }
                employees.Add(user);
            }
            task.Employees = employees;

            List<Skill> skills = new List<Skill>();
            connection();
            cmd = new SqlCommand("get_task_skills", con);//////////////////////
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@task_id", taskId);
            sd = new SqlDataAdapter(cmd);
            dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Skill skill = new Skill();
                skill.SkillId = Convert.ToInt32(dr["skill_id"]);
                skill.SkillName = Convert.ToString(dr["skill_name"]);
                skills.Add(skill);
            }

            task.Skills = skills;
            return task;
        }
        public List<Task> getTasksFromProject(int projectId){
            List<Task> tasks = new List<Task>();
            connection();
            SqlCommand cmd = new SqlCommand("get_list_of_tasks_withproject", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@project_id", projectId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                int taskId = Convert.ToInt32(dr["task_id"]);
                tasks.Add(ViewTask(taskId));
            }
            return tasks;
        }
        public List<Project> getCurrentProject(int teamleaderId)
        {
            List<Project> resultProjects = new List<Project>();
            connection();
            SqlCommand cmd = new SqlCommand("get_active_projects", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@current_date", DateTime.Today.Date);
            cmd.Parameters.AddWithValue("@teamLeaderId", teamleaderId);/////
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Project project = new Project();
                project.ProjectDepartment = new Department();
                project.Client = new Client();
                project.ProjectName = Convert.ToString(dr["project_name"]);
                project.ProjectId = Convert.ToInt32(dr["project_id"]);
                project.Client.ClientId = Convert.ToInt32(dr["client_id"]);
                project.ProjectCost = Convert.ToDouble(dr["price"]);
                project.TeamleaderID = Convert.ToInt32(dr["teamleader_id"]);
                project.ProjectDepartment.DepartmentId = Convert.ToInt32(dr["department_id"]);
                project.StartDate = Convert.ToDateTime(dr["start_date"]);
                project.EndDate = Convert.ToDateTime(dr["end_date"]);
                int projectId = project.ProjectId;

                project.Task = getTasksFromProject(projectId);

                resultProjects.Add(project);

            }
            return resultProjects;

        }

        public void endProject(int projectId)
        {

            connection();
            SqlCommand cmd = new SqlCommand("set_project_actual_end_date", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@project_id", projectId);
            cmd.Parameters.AddWithValue("@end_date", DateTime.Now.Date);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public int DepartmentId { get => departmentId; set => departmentId = value; }
        public Project Project { get => project; set => project = value; }
    }
}