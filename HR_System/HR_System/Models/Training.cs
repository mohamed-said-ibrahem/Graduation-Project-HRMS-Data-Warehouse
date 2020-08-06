using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Training
    {
        private int trainingId;
        private string trainingName;
        private DateTime startDate;
        private DateTime endDate;
        private string location;
        private int skillId;
        private int participationsNum;
        private int hoursPerDay;
        private int maxRank;
        private int positionId;
        private int departmentId;
        private int maxNumOfParticipants;
        public int TrainingId { get => trainingId; set => trainingId = value; }
        public string TrainingName { get => trainingName; set => trainingName = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public string Location { get => location; set => location = value; }
        public int SkillId { get => skillId; set => skillId = value; }
        public int ParticipationsNum { get => participationsNum; set => participationsNum = value; }
        public int HoursPerDay { get => hoursPerDay; set => hoursPerDay = value; }
        public int MaxRank { get => maxRank; set => maxRank = value; }
        public int PositionId { get => positionId; set => positionId = value; }
        public int DepartmentId { get => departmentId; set => departmentId = value; }
        public int MaxNumOfParticipants { get => maxNumOfParticipants; set => maxNumOfParticipants = value; }

        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["HRCon"].ToString();
            con = new SqlConnection(constring);
        }

        public DateTime getTrainingStartDate(int trainingId)
        {
            DateTime date=new DateTime();
            connection();
            SqlCommand cmd = new SqlCommand("getTrainingStartTime", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingId", trainingId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                date =Convert.ToDateTime(dr["start_date"]); 

            }
            return date;
        }
        public DateTime getTrainingEndDate(int trainingId)
        {
            DateTime date = new DateTime();
            connection();
            SqlCommand cmd = new SqlCommand("getTrainingEndTime", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingId", trainingId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                date = Convert.ToDateTime(dr["end_date"]);

            }
            return date;
        }
        public List<int> getAssignedEmployeesForTraining (int trainingId)
        {
            List<int> employeeList = new List<int>();
            connection();
            SqlCommand cmd = new SqlCommand("getAssignedEmployeesForTraining", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingId", trainingId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                int id = Convert.ToInt32(dr["employee_id"]);
                employeeList.Add(id);
            }
            return employeeList;
        }
    }
}