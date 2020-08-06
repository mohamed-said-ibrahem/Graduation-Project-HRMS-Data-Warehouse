using HR_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_System.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        User user = new User();
        public ActionResult viewAttendanceLog(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            int employee_id = profile.User_id;
            ViewBag.LoggedUser = profile;
            string start_date = (fc["start_date"]);
            string end_date = (fc["end_date"]);
            var attendanceLog = user.viewAttendanceDetails(start_date, end_date, employee_id);
            return View(attendanceLog);
        }

        public ActionResult logIn()
        {
            return View();
        }
        [HttpPost]

        public ActionResult logIn(FormCollection fc)
        {

            string name = fc["user"];
            string pass = fc["pass"];
            int id = user.logIn(name, pass);
            if (id > 0)
            {
                User_Info loggedUser = user.getUser(id);
                ViewBag.LoggedUser = loggedUser;
                var profileData = new User_Info
                {
                    User_id = loggedUser.User_id,
                    UserName = loggedUser.UserName,
                    Password = loggedUser.Password,
                    FullName = loggedUser.FullName,
                    Email = loggedUser.Email,
                    Salary = loggedUser.Salary,
                    PhoneNumber = loggedUser.PhoneNumber,
                    Ssn = loggedUser.Ssn,
                    Address = loggedUser.Address,
                    StartDate = loggedUser.StartDate,
                    Gender = loggedUser.Gender,
                    BirthDate = loggedUser.BirthDate,
                    EducationalDrgree = loggedUser.EducationalDrgree,
                    GraduationDate = loggedUser.GraduationDate,
                    Notes = loggedUser.Notes,
                    EmployeeSkills = loggedUser.EmployeeSkills,
                    Department = loggedUser.Department,
                    Position = loggedUser.Position,


                };

                this.Session["User"] = profileData;
                return RedirectToAction("profileInfo");

            }
            else
            {
                TempData["msg"] = "Email or Password is Wrong !";
                return View();
            }
        }

        public ActionResult profileInfo()
        {
            var profile = this.Session["User"] as User_Info;
            int id =profile.User_id ;
            User_Info user_Info = user.getUser(id);
            ViewBag.LoggedUser = profile;
            return View(user_Info);
        }
    }
}