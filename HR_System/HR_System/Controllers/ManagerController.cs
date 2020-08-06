using HR_System.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_System.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        Manager manager = new Manager();
        Utiles u = new Utiles();
       
        
        public ActionResult getSuitableEmployeesForPromotion(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            ViewBag.positions = u.position_list();
            ViewBag.departments = u.department_list();
            int depId = Convert.ToInt32(fc["DepartmentId"]);
            int posId = Convert.ToInt32(fc["PositionId"]);
            double attendance = Convert.ToDouble(fc["atten"]);
            int skillRank = Convert.ToInt32(fc["quantity"]);
            double perf = Convert.ToDouble(fc["perf"]);
            int newPos = Convert.ToInt32(fc["NewPositionId"]);
            ViewBag.newPosition = newPos;
            TempData["newPosition"] = newPos;
            DataTable dt = manager.getSuitableEmployeesForPromotion(depId, posId, skillRank, perf, attendance);
            return View(dt);
        }

        public void promote (int employeeId)
        {
            int positionId = (int)TempData["newPosition"];
            manager.changeEmployeePosition(employeeId, positionId);
        }


        public ActionResult companyPerformanceReport(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            DateTime from;
            DateTime to;
            if (string.IsNullOrEmpty(fc["from"])&& string.IsNullOrEmpty(fc["to"]))
            {
                from = Convert.ToDateTime("1/1/1753");
                to = Convert.ToDateTime("1/1/1753");

            }
            else if(string.IsNullOrEmpty(fc["from"]) && !string.IsNullOrEmpty(fc["to"]))
            {
                from = Convert.ToDateTime("1/1/1753");
                to = Convert.ToDateTime(fc["to"]);


            }
            else if (!string.IsNullOrEmpty(fc["from"]) && string.IsNullOrEmpty(fc["to"]))
            {
                to = DateTime.Now;
                from = Convert.ToDateTime(fc["from"]);

            }
            
            else
            {
                from = Convert.ToDateTime(fc["from"]);
                to = Convert.ToDateTime(fc["to"]);

            }
            DataTable dt = manager.viewCompanyPerformanceReport(from, to);
            List<DataPoint> points = new List<DataPoint>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {

                string name = Convert.ToString(dt.Rows[i]["date"]);
                double value = Convert.ToDouble(dt.Rows[i]["performance_percentage"]);
                points.Add(new DataPoint(name, value));
            }
            ViewBag.Data = JsonConvert.SerializeObject(points);

            return View(dt);

        }
        public ActionResult projectPerformanceReport()
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            ViewBag.Departments = u.department_list();
            DataTable dt = new DataTable();
            return View(dt);
        }
        [HttpPost]
        public ActionResult projectPerformanceReport(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            DateTime from;
            DateTime to;
            int departmentId;

            if (string.IsNullOrEmpty(fc["from"]) && string.IsNullOrEmpty(fc["to"]))
            {
                from = Convert.ToDateTime("1/1/1753");
                to = Convert.ToDateTime("1/1/1753");

            }
            else if (string.IsNullOrEmpty(fc["from"]) && !string.IsNullOrEmpty(fc["to"]))
            {
                from = Convert.ToDateTime("1/1/1753");
                to = Convert.ToDateTime(fc["to"]);


            }
            else if (!string.IsNullOrEmpty(fc["from"]) && string.IsNullOrEmpty(fc["to"]))
            {
                to = DateTime.Now;
                from = Convert.ToDateTime(fc["from"]);

            }

            else
            {
                from = Convert.ToDateTime(fc["from"]);
                to = Convert.ToDateTime(fc["to"]);

            }
            if (string.IsNullOrEmpty(fc["DepartmentId"]))
            {
                departmentId = 0;
            }
            else
            {
                departmentId = Convert.ToInt32(fc["DepartmentId"]);

            }
            DataTable dt = manager.viewProjectPerformanceReport(from, to, departmentId);
            List<DataPoint> points = new List<DataPoint>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {

                string name = Convert.ToString(dt.Rows[i]["project_name"]);
                double value = Convert.ToDouble(dt.Rows[i]["performance_percentage"]);
                points.Add(new DataPoint(name, value));
            }
            ViewBag.Data = JsonConvert.SerializeObject(points);
            ViewBag.Departments = u.department_list();

            return View(dt);

        }
        
        public ActionResult weaknessAndStrength(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            DateTime from;
            DateTime to;
            if (string.IsNullOrEmpty(fc["from"]) && string.IsNullOrEmpty(fc["to"]))
            {
                from = Convert.ToDateTime("1/1/1753");
                to = Convert.ToDateTime("1/1/1753");

            }
            else if (string.IsNullOrEmpty(fc["from"]) && !string.IsNullOrEmpty(fc["to"]))
            {
                from = Convert.ToDateTime("1/1/1753");
                to = Convert.ToDateTime(fc["to"]);


            }
            else if (!string.IsNullOrEmpty(fc["from"]) && string.IsNullOrEmpty(fc["to"]))
            {
                to = DateTime.Now;
                from = Convert.ToDateTime(fc["from"]);

            }

            else
            {
                from = Convert.ToDateTime(fc["from"]);
                to = Convert.ToDateTime(fc["to"]);

            }
            string category = fc["Category"];
            DataTable dt = new DataTable();
            if (category == "Weakness")
            {
                dt =  manager.viewWeaknessFieldReport(from, to);

            }else if(category== "Strength")
            {
                dt = manager.viewStrengthFieldReport(from, to);
            }
            List<DataPoint> points = new List<DataPoint>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {

                string name = Convert.ToString(dt.Rows[i]["department_name"]);
                name += "/";
                name += Convert.ToString(dt.Rows[i]["skill_name"]);
                double value = Convert.ToDouble(dt.Rows[i]["average"]);
                points.Add(new DataPoint(name, value));
            }
            ViewBag.Data = JsonConvert.SerializeObject(points);

            return View(dt);
        }
    }
    
        

}