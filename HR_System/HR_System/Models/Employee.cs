using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Employee:User
    {

        private SqlConnection con;
        private SqlConnection conDW;
        Utiles u = new Utiles();
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["HRCon"].ToString();
            con = new SqlConnection(constring);
        }

        private void DWconnection()
        {
            string constring = ConfigurationManager.ConnectionStrings["DwCon"].ToString();
            conDW = new SqlConnection(constring);
        }

        public void updateStartTaskTime(int employeeId,int TaskId ,DateTime startTime)
        {

        }
        public void updateEndTaskTime(int employeeId, int TaskId, DateTime endTime)
        {

        }
        public List<List<string>> viewTasksList (int employeeId)
        {
            List<List<string>> tasks = new List<List<string>>();

            return tasks;
        }
        public List<List<string>> viewPerformanceReport(int employeeId)
        {
            List<List<string>> performance = new List<List<string>>();

            return performance;
        }
       

        public List<Training> viewEnrolledInTraining(int employeeId)
        {
            List<Training> trainingList = new List<Training>();
            
            connection();
            SqlCommand cmd = new SqlCommand("viewTrainingForEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Training training = new Training();
                training.TrainingId = Convert.ToInt32(dr["training_id"]);
                training.TrainingName = Convert.ToString(dr["name"]);
                training.StartDate = Convert.ToDateTime(dr["start_date"]).Date;
                training.EndDate = Convert.ToDateTime(dr["end_date"]).Date;
                training.Location = Convert.ToString(dr["location"]);
                training.ParticipationsNum = Convert.ToInt32(dr["number_of_participants"]);
                training.HoursPerDay = Convert.ToInt32(dr["hours_per_day"]);
                training.SkillId = Convert.ToInt32(dr["skill_id"]);
                training.MaxRank = Convert.ToInt32(dr["maxRank"]);
                training.PositionId = Convert.ToInt32(dr["positionId"]);
                training.DepartmentId = Convert.ToInt32(dr["departmentId"]);
                training.MaxNumOfParticipants = Convert.ToInt32(dr["max_number_of_participants"]);
                trainingList.Add(training);
            }
            return trainingList;
        }

        public DataTable getFeedbackList(int employeeId)
        {

            connection();
            SqlCommand cmd = new SqlCommand("get_list_of_feedback", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;
        }
        public void giveFeedback(Feedback feedback)
        {
            connection();
            SqlCommand cmd = new SqlCommand("add_feedback", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_giver_id", feedback.EmployeeGiverId);
            cmd.Parameters.AddWithValue("@employee_reciever_id", feedback.EmployeeReceiverId);
            cmd.Parameters.AddWithValue("@task_id", feedback.TaskId);
            cmd.Parameters.AddWithValue("@skill_id", feedback.SkillId);
            cmd.Parameters.AddWithValue("@skill_rank", feedback.Rank);
            cmd.Parameters.AddWithValue("@descripition", feedback.Description);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public List<Skill> getFeedbackSkillForEmployee(int employeeId, int taskId)
        {
            List<Skill> skills = new List<Skill>();

            connection();
            SqlCommand cmd = new SqlCommand("get_skills_for_feedback", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@task_id", taskId);
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
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
                skills.Add(skill);
            }
            return skills;

        }
        public List<User_Info> getFeedbackFromTask(int employeeId, string projectName, string taskName)
        {
            List<User_Info> users = new List<User_Info>();
            connection();
            SqlCommand cmd = new SqlCommand("get_feedback_for_task", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_giver_id", employeeId);
            cmd.Parameters.AddWithValue("@project_name", projectName);
            cmd.Parameters.AddWithValue("@task_name", taskName);
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
                user.EmployeeSkills = getFeedbackSkillForEmployee(Convert.ToInt32(dr["employee_id"]), Convert.ToInt32(dr["task_id"]));
                users.Add(user);
            }

        
			return users;
			
		}
    public DataTable getWaitingTasks(int employeeId)
        {

            connection();
            SqlCommand cmd = new SqlCommand("get_list_of_tasks_not_started_by_employee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;
        }
        public DataTable getSchedulingTasks(int employeeId)
        {

            connection();
            SqlCommand cmd = new SqlCommand("get_list_of_tasks_not_finished_by_employee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;
        }
        public DataTable getActiveTraining(int employeeId)
        {

            connection();
            SqlCommand cmd = new SqlCommand("get_active_training_list", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            cmd.Parameters.AddWithValue("@current_date", DateTime.Now.Date);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;
        }

        public void set_start_time_for_task(int taskId)
        {
            connection();
            SqlCommand cmd = new SqlCommand("set_start_time_for_task", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@task_id", taskId);
            cmd.Parameters.AddWithValue("@start_time", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public DataTable viewPerformanceReportProgress(int employeeId,DateTime from, DateTime to)
        {
            List<int> id = new List<int>();
            id.Add(employeeId);
            string name = u.GetEmployeesInfo(id)[0].UserName;

            DWconnection();
           
                SqlCommand cmd = new SqlCommand("Report_performance_by_employee", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_name", name);
                cmd.Parameters.AddWithValue("@startmonth", from.Month);
                cmd.Parameters.AddWithValue("@endmonth", to.Month);
                cmd.Parameters.AddWithValue("@startyear", from.Year);
                cmd.Parameters.AddWithValue("@endyear", to.Year);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                System.Threading.Thread.Sleep(500);
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
        public DataTable viewAttendanceReportProgress(int employeeId, DateTime from, DateTime to)
        {
            List<int> id = new List<int>();
            id.Add(employeeId);
            string name = u.GetEmployeesInfo(id)[0].UserName;
            DWconnection();
            
                SqlCommand cmd = new SqlCommand("Report_attendance_by_employee", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_name", name);
                cmd.Parameters.AddWithValue("@startmonth", from.Month);
                cmd.Parameters.AddWithValue("@endmonth", to.Month);
                cmd.Parameters.AddWithValue("@startyear", from.Year);
                cmd.Parameters.AddWithValue("@endyear", to.Year);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
        }
    }


