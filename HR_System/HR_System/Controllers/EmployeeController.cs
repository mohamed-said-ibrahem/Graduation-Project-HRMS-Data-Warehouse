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
    public class EmployeeController : Controller 
    {
        User user = new User();
        Employee employee = new Employee();
        Utiles u = new Utiles();
        // GET: Employee
      
       public ActionResult getFeedbackList()
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            int employeeId = profile.User_id;
            
            var feedbackList = employee.getFeedbackList(employeeId);
            List<List<string>> tasks = new List<List<string>>();
            List<string> project = new List<string>();
            
            int j = 0;
            tasks.Add(new List<string>());
            for (int i = 0; i < feedbackList.Rows.Count; i++)
            {
                if (i == 0)
                {
                    project.Add(Convert.ToString(feedbackList.Rows[i]["project_name"]));
                    tasks[j].Add(Convert.ToString(feedbackList.Rows[i]["task_name"]));

                }
                else
                {
                    if((Convert.ToString(feedbackList.Rows[i]["project_name"])== Convert.ToString(feedbackList.Rows[i - 1]["project_name"]))){
                        tasks[j].Add(Convert.ToString(feedbackList.Rows[i]["task_name"]));

                    }
                    else
                    {
                        j++;
                        tasks.Add(new List<string>());
                        tasks[j].Add(Convert.ToString(feedbackList.Rows[i]["task_name"]));
                        project.Add(Convert.ToString(feedbackList.Rows[i]["project_name"]));

                    }
                }
            }
            ViewBag.Projects = project;
            ViewBag.Tasks = tasks;
            return View();
        }

        public ActionResult giveFeedback( string projectName, string taskName)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            int employeeId = profile.User_id;
            var users = employee.getFeedbackFromTask(employeeId, projectName, taskName);
            ViewBag.users = users;
            ViewBag.project = projectName;
            ViewBag.task = taskName;
            ViewBag.taskId = u.task_id(taskName);
            ViewBag.employee_giver_id = employeeId;
            return View();
        }

        public ActionResult saveFeedback(int employee_giver,int employee_reciever,int taskId,int skillId,int skillRank,string desc)
        {
            Feedback feedback = new Feedback();
            feedback.EmployeeGiverId = employee_giver;
            feedback.EmployeeReceiverId = employee_reciever;
            feedback.TaskId = taskId;
            feedback.SkillId = skillId;
            feedback.Rank = skillRank;
            feedback.Description = desc;
            employee.giveFeedback(feedback);

            return View();
        }
        public ActionResult viewTasks()
        {
            var profile = this.Session["User"] as User_Info;
            int employeeId = profile.User_id;
            ViewBag.LoggedUser = profile;
            DataTable scedualedTasks = employee.getSchedulingTasks(employeeId);
            DataTable waitingTasks = employee.getWaitingTasks(employeeId);
            DataTable activeTraining = employee.getActiveTraining(employeeId);
            DataSet ds = new DataSet();
            ds.Tables.Add(scedualedTasks);
            ds.Tables.Add(waitingTasks);
            ds.Tables.Add(activeTraining);
            return View(ds);
        }
        public void startTask(int taskId)
        {
            employee.set_start_time_for_task(taskId);
        }
        public ActionResult showPerformanceProgress(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            int employeeId = profile.User_id;
            DateTime from;
            DateTime to;
            if (string.IsNullOrEmpty(fc["from"])&&string.IsNullOrEmpty(fc["to"]))
            {
                from = Convert.ToDateTime("1/1/1753");
                to = Convert.ToDateTime("1/1/1753");

            }
            else
            {
                from =  Convert.ToDateTime(fc["from"]);
                to   = Convert.ToDateTime(fc["to"]);

            }


            DataTable dt = employee.viewPerformanceReportProgress(employeeId, from, to);
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
        public ActionResult showAttendanceProgress(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            int employeeId =profile.User_id;

            DateTime from = Convert.ToDateTime(fc["from"]);
            DateTime to = Convert.ToDateTime(fc["to"]);
            DataTable dt = employee.viewAttendanceReportProgress(employeeId, from, to);
            List<DataPoint> points = new List<DataPoint>();

            for (var i = 0; i < dt.Rows.Count; i++)
            {

                string name = Convert.ToString(dt.Rows[i]["month"]);
                name += "/";
                name += Convert.ToString(dt.Rows[i]["year"]);
                double value = Convert.ToDouble(dt.Rows[i]["attendance_percentage"]);
                points.Add(new DataPoint(name, value));
            }
            ViewBag.Data = JsonConvert.SerializeObject(points);

            return View(dt);



        }
    }
}
