using HR_System.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_System.Controllers
{
    public class HrAdminController : Controller
    {
        // GET: HrAdmin
        HrAdmin hr = new HrAdmin();
        Utiles u = new Utiles();
        public ActionResult viewTrainingsList()
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            var trainings = new List<Training>();
            try
            {
                trainings = hr.viewListOfTraining();
            }
            catch(Exception e)
            {

            }
            return View(trainings);
        }
        public ActionResult addTraining()
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            ViewBag.skills = u.skill_list();
            ViewBag.positions = u.position_list();
            ViewBag.departments = u.department_list();
            return View();
        }

        [HttpPost]
        public ActionResult addTraining(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            try
            {
                
                Training training = new Training();
                training.TrainingName = fc["name"];
                training.StartDate = Convert.ToDateTime(fc["start_date"]);
                training.EndDate = Convert.ToDateTime(fc["end_date"]);
                training.Location = fc["location"];
                training.HoursPerDay =Convert.ToInt32(fc["hours_per_day"]);
                training.SkillId = Convert.ToInt32(fc["SkillId"]);
                training.MaxRank = Convert.ToInt32(fc["maxRank"]);
                training.PositionId = Convert.ToInt32(fc["PositionId"]);
                training.DepartmentId = Convert.ToInt32(fc["DepartmentId"]);
                training.MaxNumOfParticipants = Convert.ToInt32(fc["maxNumPart"]);
                int training_id = hr.insertTraining(training);

                TempData["msg"] = "A New Training Has Been Added Successfully !";
            }
            catch
            {

                TempData["msg"] = "An error has been occured ,please try again !";
            }

            return RedirectToAction("addTraining");
        }

       
        public ActionResult attendance_progress (FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;

            string category = fc["Category"];
            string category_value = fc["category_value"];
            ViewBag.category_value = category_value;
            DateTime from = Convert.ToDateTime(fc["from"]);
            DateTime to = Convert.ToDateTime(fc["to"]);
            DataTable dt = hr.viewAttendanceReportProgress(category, category_value, from, to);
            List<DataPoint> points = new List<DataPoint>();

            for (var i = 0; i < dt.Rows.Count; i++)
            {

                string name = Convert.ToString(dt.Rows[i]["month"]);
                name += "/";
                name+= Convert.ToString(dt.Rows[i]["year"]); 
                double value = Convert.ToDouble(dt.Rows[i]["attendance_percentage"]);
                points.Add(new DataPoint(name, value));
            }
            ViewBag.Data = JsonConvert.SerializeObject(points);

            return View(dt);
        }

        public ActionResult attendance_comparison(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            string category = fc["Category"];
           
            DateTime date = Convert.ToDateTime(fc["date"]);
            
            DataTable dt = hr.viewAttendanceReportComparison(category,date);
            
            List<DataPoint> points = new List<DataPoint>();
            
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                
               string name =Convert.ToString(dt.Rows[i]["category_name"]);
               double value = Convert.ToDouble(dt.Rows[i]["attendance_percentage"]);
                points.Add(new DataPoint(name,value));
            }
            ViewBag.Data = JsonConvert.SerializeObject(points);
            
            return View(dt);
        }

        public ActionResult performance_progress(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            string category = fc["Category"];
            string category_value = fc["category_value"];
            ViewBag.category_value = category_value;
            DateTime from = Convert.ToDateTime(fc["from"]);
            DateTime to = Convert.ToDateTime(fc["to"]);
            DataTable dt = hr.viewPerformanceReportProgress(category, category_value, from, to);
            List<DataPoint> points = new List<DataPoint>();

            for (var i = 0; i < dt.Rows.Count; i++)
            {

                string name = Convert.ToString(dt.Rows[i]["month"]);
                name += "/";
                name += Convert.ToString(dt.Rows[i]["year"]);
                double value = Convert.ToDouble(dt.Rows[i]["performance_percentage"]);
                points.Add(new DataPoint(name, value));
            }
            ViewBag.Data = JsonConvert.SerializeObject(points);

            return View(dt);
        }

        public ActionResult performance_comparison(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            string category = fc["Category"];

            DateTime date = Convert.ToDateTime(fc["date"]);

            DataTable dt = hr.viewPerformanceReportComparison(category, date);
            List<DataPoint> points = new List<DataPoint>();

            for (var i = 0; i < dt.Rows.Count; i++)
            {

                string name = Convert.ToString(dt.Rows[i]["category_name"]);
                double value = Convert.ToDouble(dt.Rows[i]["performance_percentage"]);
                points.Add(new DataPoint(name, value));
            }
            ViewBag.Data = JsonConvert.SerializeObject(points);

            return View(dt);
        }

        public ActionResult viewTraining(int training_id )
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            Training training = hr.viewTraining(training_id);
            ViewBag.Training = training;
            ViewBag.skill_name = u.skill_name(training.SkillId);
            if(training.DepartmentId > 0)
            {
                ViewBag.Department_name = u.department_name(training.DepartmentId);
            }
            if(training.PositionId > 0)
            {
                ViewBag.Position_name = u.position_info(training.PositionId).PositionName;
            }
            DataTable dt = hr.employeeEnrolledInTraining(training_id);
            List<User_Info> recommendedEmployees = u.GetEmployeesInfo(hr.viewEmployeesForTraining(training_id));
            ViewBag.recommendedEmployees = recommendedEmployees;
            return View(dt);
        }
        public JsonResult assignTrainingToEmloyee(int trainingId, int employeeId)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            int value = hr.assignTrainingToEmloyee(trainingId, employeeId);
            return Json(value);
        }
            public void unassignEmployeeFromTraining(int training_id,int employee_id)
        {
            hr.unAssignEmployeeFromTraining(training_id, employee_id);

            
        }
        public JsonResult addEmployeeToTrainingByName(string userName,int trainingId)
        {
            int value = -1;

            try
            {
                value = hr.addEmployeeToTrainingByName(userName, trainingId);
                return Json(value);
            }
            catch(Exception e)
            {
                
            }
            return Json(value);
        }

        public ActionResult setBounaceCriteria()
        {
            DataTable dt = new DataTable();
            ViewBag.departments = u.department_list();
            ViewBag.positions = u.position_list();
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            return View();
        }
        [HttpPost]
        public ActionResult setBounaceCriteria(FormCollection fc)
        {

            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            ViewBag.departments = u.department_list();
            ViewBag.positions = u.position_list();
            Bonus bonus = new Bonus();
            bonus.DepartmentId =Convert.ToInt32(fc["DepartmentId"]);
            bonus.PositionId = Convert.ToInt32(fc["PositionId"]);
            bonus.MinSkillsRank = Convert.ToInt32(fc["quantity"]);
            bonus.MinPerformancePercentage = Convert.ToDouble(fc["performance"]);
            bonus.MinAttendancePercentage = Convert.ToDouble(fc["atten"]);
            bonus.BonusValue = Convert.ToDouble(fc["bonus"]);
            bonus.Description = Convert.ToString(fc["desc"]);
            DataTable dt = hr.setBounaceCriteria(bonus);
            return View(dt);
        }

        public ActionResult hireApplicant()
        {
            ViewBag.Applicants = u.applicants_list();
            ViewBag.Department = u.department_list();
            ViewBag.Position = u.position_list();
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            return View();
        }
        [HttpPost]
        public ActionResult hireApplicant(FormCollection fc)
        {

            ViewBag.Applicants = u.applicants_list();
            ViewBag.Department = u.department_list();
            ViewBag.Position = u.position_list();
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            int applicantId = Convert.ToInt32(fc["ApplicantId"]);
            int positionId = Convert.ToInt32(fc["PositionId"]);
            int departmentId = Convert.ToInt32(fc["DepartmentId"]);
            string tempUserName = fc["user"];
            string tempPassword = fc["pass"];
            double salary = Convert.ToDouble(fc["salary"]);
            DateTime birth_date = Convert.ToDateTime(fc["birthDate"]);
            string educational_degree = fc["edu"];
            string graduation_date = fc["grad"];
            string notes = fc["notes"];
            hr.hireApplicant(applicantId, positionId, departmentId, tempUserName, tempPassword, salary, birth_date, educational_degree, graduation_date, notes);
            return View();
        }

        public ActionResult holidays()
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            return View();
        }

        public ActionResult addHolidday()
        {

            return View();
        }
        [HttpPost]
        public ActionResult addHoliday(FormCollection fc)
        {
            int hr_id = 41;
            Holiday holiday = new Holiday();
            holiday.EmployeeName = fc["name"];
            holiday.StartDate = Convert.ToDateTime(fc["start"]);
            holiday.EndDate = Convert.ToDateTime(fc["end"]);
            holiday.Cause = fc["cause"];
            hr.addHoliday(holiday, hr_id);
            return View();
        }
        public ActionResult addVacation()
        {

            return View();
        }
        [HttpPost]
        public ActionResult addVacation(FormCollection fc)
        {
            int hr_id = 41;
            Official_Vacation vacation = new Official_Vacation();
            vacation.VacationName = fc["name"];
            vacation.StartDate = Convert.ToDateTime(fc["start"]);
            vacation.EndDate = Convert.ToDateTime(fc["end"]);

            hr.addVacation(vacation);
            return View();
        }
        public ActionResult addPermission()
        {

            return View();
        }
        [HttpPost]
        public ActionResult addPermission(FormCollection fc)
        {
           
                int hr_id = 41;
                Permission permission = new Permission();
                permission.EmployeeName = fc["name"];
                DateTime dateOnly = Convert.ToDateTime(fc["date"]);
                DateTime timeOnly = Convert.ToDateTime(fc["start"]);
                permission.StartTime = dateOnly.Date.Add(timeOnly.TimeOfDay);
                timeOnly = Convert.ToDateTime(fc["end"]);
                permission.EndTime = dateOnly.Date.Add(timeOnly.TimeOfDay);
                permission.Cause = fc["cause"];
                hr.addPermission(permission, hr_id);
           
            return View();
        }
    }
}