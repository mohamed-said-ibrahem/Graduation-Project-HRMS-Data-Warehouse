using HR.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class User:User_Info
    {
        private int user_id;
        private int department_id;
        private string position;
        private string name;
        private string email;
        private string number;
        private string ssn;
        private string address;
        Utiles u = new Utiles();
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["HRCon"].ToString();
            con = new SqlConnection(constring);
        }

        public int User_id { get => user_id; set => user_id = value; }

        private void updateProfile(int user_id, LinkedList<string> newValues)
        {

        }
        public int logIn(string userName, string password)
        {
            connection();
            int id = 0;
           
            SqlCommand cmd = new SqlCommand("logIn", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.AddWithValue("@password", (password));

            cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            if (!DBNull.Value.Equals(cmd.Parameters["@id"].Value))
            {
                id = Convert.ToInt32(cmd.Parameters["@id"].Value);
            }
            else
            {
                id = 0;

            }
            
            return id;

        }
        private void logOut(int id)
        {

        }
        // check if the employee has set his arrival time before
        public int checkDuplicatesArrivals(int employeeId, DateTime date)
        {
            connection();
            SqlCommand cmd = new SqlCommand("checkDuplicatesArrivals", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.Add("@count", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            int count = Convert.ToInt32(cmd.Parameters["@count"].Value);
            return count;
        }
        private int setArrivalTime(int id)
        {
            DateTime now = DateTime.Now;
            int success = 0;
            if (checkDuplicatesArrivals(id, now.Date) == 0)
            {
                connection();
                SqlCommand cmd = new SqlCommand("setArrivalTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employeeId", id);
                cmd.Parameters.AddWithValue("@date", now.Date);
                cmd.Parameters.AddWithValue("@time", now.TimeOfDay);

                con.Open();
                success = cmd.ExecuteNonQuery();
                con.Close();
                return success;
            }
            return success;
        }
        // check whether the employee has already set his leave time before
        public int checkDuplicatesLeave(int employeeId, DateTime date)
        {
            connection();
            SqlCommand cmd = new SqlCommand("checkDuplicatesLeave", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.Add("@count", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            int count = Convert.ToInt32(cmd.Parameters["@count"].Value);
            return count;
        }
        private int setLeaveTime(int id)
        {
            DateTime now = DateTime.Now;
            int success = 0;
            if (checkDuplicatesLeave(id, now.Date) == 0)
            {
                connection();
                SqlCommand cmd = new SqlCommand("setLeaveTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employeeId", id);
                cmd.Parameters.AddWithValue("@date", now.Date);
                cmd.Parameters.AddWithValue("@time", now.TimeOfDay);

                con.Open();
                success = cmd.ExecuteNonQuery();
                con.Close();
                return success;
            }
            return success;
        }
        //if start_date and end_date == null then view the attendance of the previous month
        //start_date == null view attendance of a month before end_date
        //end_date == null view attendance of start_date only
        public List<Attendance> viewAttendance(int id, DateTime start_date, DateTime end_date)
        {
            List<Attendance> attendancesList = new List<Attendance>();
            start_date = start_date.Date;
            end_date = end_date.Date;


            if (start_date == null && end_date == null)
            {
                end_date = DateTime.Today.Date;
                start_date = end_date.AddDays(-30);
            } else if (start_date == null)
            {
                start_date = end_date.AddDays(-30);
            }
            else if (end_date == null)
            {
                end_date = start_date;
            }
            connection();
            SqlCommand cmd = new SqlCommand("viewEmployeeAttendance", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Attendance attendance = new Attendance();
                attendance.Date = Convert.ToDateTime(dr["day_date"]).Date;
                attendance.ArrivalTime = (TimeSpan)Convert.ToDateTime(dr["arrival_time"]).TimeOfDay;
                attendance.LeaveTime = (TimeSpan)Convert.ToDateTime(dr["leave_time"]).TimeOfDay;

                attendancesList.Add(attendance);
            }


            return attendancesList;
        }

        //Attendance update 
        //take start and end date and get Attendance table with permissions and holidays 
        public List<Attendance> viewAttendanceDetails(string taken_start_date, string taken_end_date, int employeeId)
        {


            int attendanceIndex = 0;
            int holidayIndex = 0;
            int permissionIndex = 0;
            int vacationIndex = 0;

            List<Attendance> attendanceLog = new List<Attendance>();
            // start_date = start_date.Date;
            // end_date = end_date.Date;

            DateTime start_date = new DateTime();

            DateTime end_date = new DateTime();
            if (((taken_start_date == null) || (taken_start_date == "")) && ((taken_end_date == null) || (taken_end_date == "")))
            {
                DateTime now = DateTime.Now;
                start_date = new DateTime(now.Year, now.Month, 1);
                end_date = start_date.AddMonths(1).AddDays(-1);
            }
            else if ((taken_start_date == null)||(taken_start_date==""))
            {
                DateTime now = DateTime.Now;
                start_date = new DateTime(now.Year, now.Month, 1);
                end_date = start_date.AddMonths(1).AddDays(-1);
                
            }
            else if ((taken_end_date == null)||(taken_end_date==""))
            {
                start_date = Convert.ToDateTime(taken_start_date).Date;
                end_date = start_date;
            }
            else
            {
                start_date = Convert.ToDateTime(taken_start_date).Date;
                end_date = Convert.ToDateTime(taken_end_date).Date;
            }
            List<Attendance> attendanceList = getAttendance(employeeId, start_date, end_date);
            List<Holiday> holidays = getHolidays(employeeId, start_date, end_date);
            List<Permission> permissions = getPermissions(employeeId, start_date, end_date);
            List<Official_Vacation> official_Vacations = getOfficialVacations(start_date, end_date);
            for (DateTime day = start_date; day.Date <= end_date.Date; day = day.AddDays(1))
            {
                bool flagAttendance = false;
                if(attendanceIndex < attendanceList.Count)
                {
                    if (day.Date.Equals(attendanceList[attendanceIndex].Date))
                    {
                        if (permissionIndex < permissions.Count)
                        {
                            if (day.Date.Equals(permissions[permissionIndex].StartTime.Date))
                            {
                                string note = String.Concat("Permission : from ", permissions[permissionIndex].StartTime.ToString("r", DateTimeFormatInfo.InvariantInfo), " to", permissions[permissionIndex].EndTime.ToString("r", DateTimeFormatInfo.InvariantInfo));
                                attendanceLog.Add(getAttendanceLog(attendanceList[attendanceIndex].ArrivalTime, attendanceList[attendanceIndex].LeaveTime, attendanceList[attendanceIndex].Date, note));
                                permissionIndex++;
                            }
                            else
                            {
                                attendanceLog.Add(getAttendanceLog(attendanceList[attendanceIndex].ArrivalTime, attendanceList[attendanceIndex].LeaveTime, attendanceList[attendanceIndex].Date, "Attendance"));
                            }
                        }
                        else
                        {
                            attendanceLog.Add(getAttendanceLog(attendanceList[attendanceIndex].ArrivalTime, attendanceList[attendanceIndex].LeaveTime, attendanceList[attendanceIndex].Date, "Attendance"));
                        }
                        attendanceIndex++;
                        flagAttendance = true;

                    }
                    else
                    {
                        bool flagVacation = false;
                        if (vacationIndex < official_Vacations.Count)
                        {
                            if ((day.Date.CompareTo(official_Vacations[vacationIndex].StartDate)>=0)&& (day.Date.CompareTo(official_Vacations[vacationIndex].EndDate) <= 0) )
                            {
                                flagVacation = true;
                                attendanceLog.Add(getAttendanceLog(TimeSpan.Zero, TimeSpan.Zero, day.Date, official_Vacations[vacationIndex].VacationName));
                                if(day.Date.CompareTo(official_Vacations[vacationIndex].EndDate) == 0)
                                {
                                    vacationIndex++;

                                }
                            }


                        }
                        if (!flagVacation)
                        {
                            bool flagHoliday = false;
                            if(holidayIndex < holidays.Count)
                            {
                                if ((day.Date.CompareTo(holidays[holidayIndex].StartDate) >= 0) && (day.Date.CompareTo(holidays[holidayIndex].EndDate) <= 0))
                                {
                                    flagHoliday = true;
                                    attendanceLog.Add(getAttendanceLog(TimeSpan.Zero, TimeSpan.Zero, day.Date, "Paied Holiday"));
                                    if (day.Date.CompareTo(holidays[holidayIndex].EndDate) == 0)
                                    {
                                        holidayIndex++;

                                    }
                                }

                            }
                            if (!flagHoliday)
                            {
                                attendanceLog.Add(getAttendanceLog(TimeSpan.Zero, TimeSpan.Zero, day.Date, "Absent"));
                            }
                        }
                     }

                }
                else
                {
                    bool flagVacation = false;
                    if (vacationIndex < official_Vacations.Count)
                    {
                        if ((day.Date.CompareTo(official_Vacations[vacationIndex].StartDate) >= 0) && (day.Date.CompareTo(official_Vacations[vacationIndex].EndDate) <= 0))
                        {
                            flagVacation = true;
                            attendanceLog.Add(getAttendanceLog(TimeSpan.Zero, TimeSpan.Zero, day.Date, official_Vacations[vacationIndex].VacationName));
                            if (day.Date.CompareTo(official_Vacations[vacationIndex].EndDate) == 0)
                            {
                                vacationIndex++;

                            }
                        }


                    }
                    if (!flagVacation)
                    {
                        bool flagHoliday = false;
                        if (holidayIndex < holidays.Count)
                        {
                            if ((day.Date.CompareTo(holidays[holidayIndex].StartDate) >= 0) && (day.Date.CompareTo(holidays[holidayIndex].EndDate) <= 0))
                            {
                                flagHoliday = true;
                                attendanceLog.Add(getAttendanceLog(TimeSpan.Zero, TimeSpan.Zero, day.Date, "Paied Holiday"));
                                if (day.Date.CompareTo(holidays[holidayIndex].EndDate) == 0)
                                {
                                    holidayIndex++;

                                }
                            }

                        }
                        if (!flagHoliday)
                        {
                            attendanceLog.Add(getAttendanceLog(TimeSpan.Zero, TimeSpan.Zero, day.Date, "Absent"));
                        }
                    }
                }

            }

                return attendanceLog;
        }

        private Attendance getAttendanceLog(TimeSpan arrivalTime, TimeSpan leaveTime, DateTime date, string v)
        {
            Attendance attendance = new Attendance();
            attendance.ArrivalTime = arrivalTime;
            attendance.LeaveTime = leaveTime;
            attendance.Date = date;
            attendance.Note = v;

            return attendance;
        }

        private List<Official_Vacation> getOfficialVacations(DateTime start_date, DateTime end_date)
        {
            List<Official_Vacation> official_Vacations = new List<Official_Vacation>();
            connection();
            
            SqlCommand cmd = new SqlCommand("Employee_official_vacation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@startdate", start_date.Date);
            cmd.Parameters.AddWithValue("@enddate", end_date.Date);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Official_Vacation official_Vacation = new Official_Vacation();
                official_Vacation.StartDate = Convert.ToDateTime(dr["start_day"]).Date;
                official_Vacation.EndDate = Convert.ToDateTime(dr["end_day"]).Date;
                official_Vacation.VacationName = Convert.ToString(dr["name"]);
                official_Vacations.Add(official_Vacation);

            }
            return official_Vacations;
        }

        private List<Permission> getPermissions(int employeeId, DateTime start_date, DateTime end_date)
        {
            List<Permission> permissions = new List<Permission>();
            connection();

            SqlCommand cmd = new SqlCommand("Employee_permission", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            cmd.Parameters.AddWithValue("@startdate", start_date.Date);
            cmd.Parameters.AddWithValue("@enddate", end_date.Date);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Permission permission = new Permission();
                permission.StartTime = Convert.ToDateTime(dr["start_time"]);
                permission.EndTime = Convert.ToDateTime(dr["end_time"]);

                permissions.Add(permission);

            }
            return permissions;
        }

        private List<Holiday> getHolidays(int employeeId, DateTime start_date, DateTime end_date)
        {
            List<Holiday> holidays = new List<Holiday>();
            connection();

            SqlCommand cmd = new SqlCommand("Employee_Holiday", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            cmd.Parameters.AddWithValue("@startdate", start_date.Date);
            cmd.Parameters.AddWithValue("@enddate", end_date.Date);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Holiday holiday = new Holiday();
                holiday.StartDate = Convert.ToDateTime(dr["start_date"]).Date;
                holiday.EndDate = Convert.ToDateTime(dr["end_date"]).Date;

                holidays.Add(holiday);

            }
            return holidays;
        }

        private List<Attendance> getAttendance(int employeeId, DateTime start_date, DateTime end_date)
        {
            List<Attendance> attendances = new List<Attendance>();
            connection();

            SqlCommand cmd = new SqlCommand("Employee_attendance", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_id", employeeId);
            cmd.Parameters.AddWithValue("@startdate", start_date.Date);
            cmd.Parameters.AddWithValue("@enddate", end_date.Date);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Attendance attendance = new Attendance();
                attendance.ArrivalTime = (TimeSpan)(dr["arrival_time"]);
                attendance.LeaveTime =(TimeSpan) (dr["leave_time"]);
                attendance.Date = Convert.ToDateTime(dr["day_date"]).Date;
                attendances.Add(attendance);

            }
            return attendances;
        }

        private List<string> viewProfile(int id)
        {
            List<string> profileInfo = new List<string>();

            return profileInfo;
        }

       // public void signInTemp()
        public User_Info getUser(int id)
        {
            connection();
            User_Info user = new User_Info();
            
            SqlCommand cmd = new SqlCommand("userInfo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id",id);
            

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                user.User_id = Convert.ToInt32(dr["employee_id"]);
                user.UserName = Convert.ToString(dr["user_name"]);
                Department department = new Department();
                department.DepartmentId = Convert.ToInt32(dr["department_id"]);
                department.Name = Convert.ToString(dr["department_name"]);
                department.ManagerId = Convert.ToInt32(dr["manager_id"]);
                user.Department = department;

                Position position = new Position();
                position.PositionId = Convert.ToInt32(dr["position_id"]);
                position.PositionName = Convert.ToString(dr["position_name"]);
                position.PositionDescription = Convert.ToString(dr["position_description"]);
                user.Position = position;

                user.FullName = Convert.ToString(dr["full_name"]);
                user.Email = Convert.ToString(dr["email"]);
                user.Salary = Convert.ToDouble(dr["salary"]);
                user.PhoneNumber = Convert.ToString(dr["phone_number"]);
                user.Ssn = Convert.ToString(dr["ssd"]);
                user.Address = Convert.ToString(dr["address"]);
                user.StartDate = Convert.ToDateTime(dr["start_date"]);
                user.Gender = Convert.ToString(dr["gender"]);
                user.BirthDate = Convert.ToDateTime(dr["birth_date"]);
                user.EducationalDrgree = Convert.ToString(dr["educational_degree"]);
                user.GraduationDate = Convert.ToDateTime(dr["graduation_date"]);
                user.Notes = Convert.ToString(dr["notes"]);
                user.EmployeeSkills = getEmployeeSkills(Convert.ToInt32(dr["employee_id"]));
            }
            return user;
        }
        public List<Skill> getEmployeeSkills(int employeeId)
        {

            List<Skill> skills = new List<Skill>();

            connection();
            SqlCommand cmd = new SqlCommand("get_skills_for_employee", con);
            cmd.CommandType = CommandType.StoredProcedure;
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
                skill.SkillDescription = Convert.ToString(dr["description"]);
                skill.Rate = Convert.ToInt32(dr["skill_rank"]);
                skills.Add(skill);
            }
            return skills;

        }
    }
}