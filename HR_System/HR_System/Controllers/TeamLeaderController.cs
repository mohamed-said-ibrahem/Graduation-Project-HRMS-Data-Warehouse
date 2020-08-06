using HR_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_System.Controllers
{
    public class TeamLeaderController : Controller
    {
        TeamLeader leader = new TeamLeader();
        Utiles u = new Utiles();
        // GET: TeamLeader
        public ActionResult projectsList()
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            int teamLeader = profile.User_id;
            var projects = leader.viewAllProject(teamLeader);
            string name = u.GetEmployeesInfo(new List<int>{teamLeader})[0].UserName;
            ViewBag.LeaderName = name;
            ViewBag.departments = u.department_list();
            ViewBag.clients = u.client_list();
            return View(projects);
        }
        public void create_project()
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
        }
        [HttpPost]
        public void create_project(FormCollection fc)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            Project project = new Project();
            project.ProjectDepartment = new Department();
            project.Client = new Client();
            //from session
            project.StartDate = DateTime.Now;
            
           
            string name = fc["name"];
            double cost = Convert.ToDouble(fc["cost"]);
            DateTime date = Convert.ToDateTime(fc["date"]);
            int depId = Convert.ToInt32(fc["DepartmentId"]);
            int c_id = Convert.ToInt32(fc["ClientId"]);
            project.ProjectName = name.Substring(0,name.Length-2);
            project.EndDate = date;

            project.ProjectCost = cost;
            project.TeamleaderID = profile.User_id;
            project.ProjectDepartment.DepartmentId = depId;
            project.Client.ClientId = c_id;
           

            leader.createProject(project);
            
        }

        public ActionResult getCurrentProject()
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            int teamLeaderId = profile.User_id;
            var projects = leader.getCurrentProject(teamLeaderId);

            return View(projects);
        }
       
        public ActionResult createTask()
        {
            ViewBag.Skills = u.skill_list();
            ViewBag.Projects = u.project_list();
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;

            return View();
        }
        [HttpPost]
        public ActionResult createTask(FormCollection fc)
        {

            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            Task task = new Task();
            task.Skills = new List<Skill>();
            task.TaskName = fc["task_name"];
            task.ProjectId =Convert.ToInt32(fc["ProjectId"]);
            task.AssignDate = Convert.ToDateTime(fc["assign_time"]);
            task.Deadline = Convert.ToDateTime(fc["deadline"]);
            List < string > skills = fc["SkillId"].Split(',').ToList();
           skills.Select(int.Parse).ToList();
            for(int i = 0; i < skills.Count; i++)
            {
                Skill skill = new Skill();
                skill.SkillId =Convert.ToInt32(skills[i]);
                task.Skills.Add(skill);
               
            }
            
           // int taskId = leader.createTask(task);
            
            //task.TaskId = taskId;

            TempData["task"] = task;
            return RedirectToAction("AssignEmployees");
        }
        public void saveTask()
        {
            Task task = (Task)TempData["task"];
            int taskId = leader.createTask(task);

            task.TaskId = taskId;
            TempData["task"] = task;
        }
        public ActionResult AssignEmployees()
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            Task assignTask = (Task)TempData["task"];
            var employees = leader.getRecommendedEmployeeForTask(assignTask);
            ViewBag.Employees = employees;
            ViewBag.TaskId = assignTask.TaskId;
            ViewBag.Skills = u.skill_list();
            TempData["task"] = assignTask;
            return View();
        }

        public JsonResult AssignEmployeeForTask( int employeeId,int taskId)
        {

            Task assignTask = (Task)TempData["task"];
            // int taskId = assignTask.TaskId;
            int value = -1;
            value=leader.AssignEmployeeForTask(assignTask.TaskId,employeeId, assignTask.AssignDate, assignTask.Deadline);
            TempData["task"] = assignTask;
            return Json(value);
        }
        public JsonResult AssignEmployeeToTaskByName(string name, int taskId)
        {

            Task assignTask = (Task)TempData["task"];
            // int taskId = assignTask.TaskId;
            int value = -1;
            int employeeId =u.getIdByUserName(name);
            value=  leader.AssignEmployeeForTask(taskId, employeeId,assignTask.AssignDate,assignTask.Deadline);
            TempData["task"] = assignTask;

            return Json(value);
        }
        public void setEmployeeTask(int employeeId,string skill, int taskId) 
        {
            //Task assignTask = (Task)TempData["task"];
            //int taskId = assignTask.TaskId;
            List<string> skills = skill.Split(',').ToList();

            leader.setEmployeeTask(employeeId, taskId, skills.Select(int.Parse).ToList());
        }

        public ActionResult viewTask(int taskId=1)
        {
            var profile = this.Session["User"] as User_Info;
            ViewBag.LoggedUser = profile;
            Task task = leader.ViewTask(taskId);
            task.TaskId = taskId;
            ViewBag.ProjectName = u.getProjectName(task.ProjectId);
            return View(task);
        }
        public void EndTask(int taskId)
        {
            leader.addTaskEndTime(taskId);
        }
        public void endProject(int projectId)
        {
            leader.endProject(projectId);
        }
    }
    
}