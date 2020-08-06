using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class ApplicantForm
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["HRCon"].ToString();
            con = new SqlConnection(constring);
        }

        public void addApplicant(Applicant applicant)
        {
            int applicantId = 0;
            connection();
            SqlCommand cmd = new SqlCommand("add_applicant", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@full_name", applicant.Name);
            cmd.Parameters.AddWithValue("@email", applicant.Email);
            cmd.Parameters.AddWithValue("@phone_number", applicant.Number);
            cmd.Parameters.AddWithValue("@ssn", applicant.Ssn);
            cmd.Parameters.AddWithValue("@address", applicant.Address);
            cmd.Parameters.AddWithValue("@education", applicant.Education);
            cmd.Parameters.AddWithValue("@graduation_year", applicant.GraduationYear);
            cmd.Parameters.AddWithValue("@experience", applicant.YearsOfExperience);
            cmd.Parameters.AddWithValue("@gender", applicant.Gendre);
            con.Open();

            cmd.ExecuteNonQuery();
            applicantId = Convert.ToInt32(cmd.Parameters["@applicant_id"].Value);
            con.Close();

            for (int i = 0; i < applicant.Skills.Count; i++)
            {
                connection();
                cmd = new SqlCommand("add_applicant_skill", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@applicant_id", applicantId);
                cmd.Parameters.AddWithValue("@skill_id", applicant.Skills[i].SkillId);
                cmd.Parameters.AddWithValue("@skill_rank", applicant.Skills[i].Rate);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}