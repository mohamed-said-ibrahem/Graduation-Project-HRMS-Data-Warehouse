using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace HR_System.Models
{
    public class Manager:Employee
    {
        private SqlConnection con;

        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["HRCon"].ToString();
            con = new SqlConnection(constring);
        }
        private SqlConnection conDW;
        private void DWconnection()
        {
            string constring = ConfigurationManager.ConnectionStrings["DwCon"].ToString();
            conDW = new SqlConnection(constring);
        }

        public DataTable viewCompanyPerformanceReport(DateTime from, DateTime to)
        {
            connection();
            SqlCommand cmd = new SqlCommand("list_project_performance", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@start_date", from.Date);
            cmd.Parameters.AddWithValue("@end_date", to.Date);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();

            con.Open();
            sd.Fill(dt1);
            con.Close();


            DWconnection();
            cmd = new SqlCommand("list_employee_performance", conDW);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@start_date", from.Date);
            cmd.Parameters.AddWithValue("@end_date", to.Date);

            sd = new SqlDataAdapter(cmd);
            DataTable dt2 = new DataTable();

            conDW.Open();
            sd.Fill(dt2);
            conDW.Close();

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("date");
            dtResult.Columns.Add("performance_percentage");
            DataRow drResult = null;

            int index1 = 0, index2 = 0;
            for (DateTime date = from; date < to; date = date.AddMonths(1))
            {
                int value1 = 0, value2 = 0;

                // DateTime date1 = new DateTime();

                // date1.AddMonths(Convert.ToInt32(dt1.Rows[index1][0]));
                // date1.AddYears(Convert.ToInt32(dt1.Rows[index1][1]));
                if (index1 < dt1.Rows.Count)
                {
                    if ((date.Year == Convert.ToInt32(dt1.Rows[index1][1])) && (date.Month == Convert.ToInt32(dt1.Rows[index1][0])))
                    {
                        value1 = Convert.ToInt32(dt1.Rows[index1][2]);
                        index1++;
                    }
                }
                // DateTime date2 = new DateTime();
                // date2.AddMonths(Convert.ToInt32(dt2.Rows[index2][0]));
                // date2.AddYears(Convert.ToInt32(dt2.Rows[index2][1]));
                if (index2 < dt2.Rows.Count)
                {
                    if ((date.Year == Convert.ToInt32(dt2.Rows[index2][1])) && (date.Month == Convert.ToInt32(dt2.Rows[index2][0]))
    )
                    {
                        value2 = Convert.ToInt32(dt2.Rows[index2][2]);
                        index2++;
                    }
                }
                double performance = 0.7 * value1 + 0.3 * value2;


                drResult = dtResult.NewRow(); // have new row on each iteration
                drResult["date"] = date.ToString();
                drResult["performance_percentage"] = Convert.ToString(performance);
                dtResult.Rows.Add(drResult);

            }

            return dtResult;
        }

        public DataTable viewProjectPerformanceReport(DateTime from, DateTime to, int departmentId)
        {
            connection();
            SqlCommand cmd = new SqlCommand("get_project_performance", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@department_id", departmentId);
            cmd.Parameters.AddWithValue("@start_date", from.Date);
            cmd.Parameters.AddWithValue("@end_date", to.Date);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            return dt;
        }


        public DataTable viewWeaknessFieldReport(DateTime from, DateTime to)
        {
            connection();
            SqlCommand cmd = new SqlCommand("weakness_field", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@start_date", from.Date);
            cmd.Parameters.AddWithValue("@end_date", to.Date);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            List<String> skillsNames = new List<String>();
            foreach (DataRow dr in dt.Rows)
            {
                connection();
                cmd = new SqlCommand("get_last_skills_for_department", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@department_id", Convert.ToInt32(dr["department_id"]));
                cmd.Parameters.AddWithValue("@start_date", from.Date);
                cmd.Parameters.AddWithValue("@end_date", to.Date);
                sd = new SqlDataAdapter(cmd);
                DataTable dt2 = new DataTable();

                con.Open();
                sd.Fill(dt2);
                con.Close();
                String skills = "";
                foreach (DataRow dr2 in dt2.Rows)
                {
                    skills += Convert.ToString(dr2["skill_name"]);
                    skills += ",";

                }
                skillsNames.Add(skills);
            }


            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("department_name");
            dtResult.Columns.Add("skill_name");
            dtResult.Columns.Add("average");
            DataRow drResult = null;
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                drResult = dtResult.NewRow(); // have new row on each iteration
                drResult["department_name"] = dr["department_name"];
                drResult["skill_name"] = skillsNames[i];
                drResult["average"] = dr["average"];

                dtResult.Rows.Add(drResult);
                i++;
            }

            
            return dtResult;
        }
        public DataTable viewStrengthFieldReport(DateTime from, DateTime to)
        {
            connection();
            SqlCommand cmd = new SqlCommand("strength_field", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@start_date", from.Date);
            cmd.Parameters.AddWithValue("@end_date", to.Date);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            List<String> skillsNames = new List<String>();
            foreach (DataRow dr in dt.Rows)
            {
                connection();
                cmd = new SqlCommand("get_top_skills_for_department", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@department_id", Convert.ToInt32(dr["department_id"]));
                cmd.Parameters.AddWithValue("@start_date", from.Date);
                cmd.Parameters.AddWithValue("@end_date", to.Date);
                sd = new SqlDataAdapter(cmd);
                DataTable dt2 = new DataTable();

                con.Open();
                sd.Fill(dt2);
                con.Close();
                String skills = "";
                foreach (DataRow dr2 in dt2.Rows)
                {
                    skills += Convert.ToString(dr2["skill_name"]);
                    skills += ",";
                }
                skillsNames.Add(skills);
            }


            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("department_name");
            dtResult.Columns.Add("skill_name");
            dtResult.Columns.Add("average");
            DataRow drResult = null;
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                drResult = dtResult.NewRow(); // have new row on each iteration
                drResult["department_name"] = dr["department_name"];
                drResult["skill_name"] = skillsNames[i];
                drResult["average"] = dr["average"];

                dtResult.Rows.Add(drResult);
                i++;
            }

            return dtResult;
        }

        public DataTable getSuitableEmployeesForPromotion(int departmentId, int positionId, int minSkillRank, double minPerformance, double minAttendance)
        {

            connection();
            SqlCommand cmd = new SqlCommand("get_suitable_employee_for_promotion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@department_id", departmentId);
            cmd.Parameters.AddWithValue("@position_id", positionId);
            cmd.Parameters.AddWithValue("@skill_rank", minSkillRank);
            cmd.Parameters.AddWithValue("@performance", minPerformance);
            cmd.Parameters.AddWithValue("@attendance", minAttendance);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;

        }
        public void changeEmployeePosition(int employeeId, int newPositionId)
        {
            connection();
            SqlCommand cmd = new SqlCommand("change_employee_position", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            cmd.Parameters.AddWithValue("@new_position_id", newPositionId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }
       /* public void loadBackup()
        {
            string pkgLocation;
            Package pkg;
            Application app;
            DTSExecResult pkgResults;

            pkgLocation =
              @"C:\Program Files\Microsoft SQL Server\100\Samples\Integration Services" +
              @"\Package Samples\CalculatedColumns Sample\CalculatedColumns\CalculatedColumns.dtsx";
            app = new Application();
            pkg = app.LoadPackage(pkgLocation, null);
            pkgResults = pkg.Execute();

            Console.WriteLine(pkgResults.ToString());
            Console.ReadKey();
        }*/
    }
}
