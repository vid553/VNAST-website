using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNASTWebsite.Models;

namespace VNASTWebsite.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
       //     APIController api = AccountController.apiRequestController;
         //   string json = api.RequestGet("/me");


            
            string get_currentUser = AccountController.apiRequestController.RequestGet("me");
            var currentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(get_currentUser);
           
            
            //if (post_login)
            //{
            //    string get_user = apiRequestController.RequestGet("me");
            //}
            int c = 0;
            if (currentUser.privilege[0]== "admin")
            {
                string get_users = AccountController.apiRequestController.RequestGet("users");
                var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(get_users);
                currentUser.workers = users;
                return View(currentUser);
            }
            else if (currentUser.privilege[0] == "worker")
            {
                string get_userTasks = AccountController.apiRequestController.RequestGet("tasks/get/mytasks");
                var userTasks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Assignment>>(get_userTasks);
                currentUser.Assignments = userTasks;
                return View(currentUser);
            }
            return View(currentUser);
    /*        List<Assignment> list = new List<Assignment>();
            list.Add(new Assignment("NAME1", DateTime.Parse("24.10.2018"), DateTime.Parse("30.11.2018"), "ACTIVE"));
            list.Add(new Assignment("NAME2", DateTime.Parse("25.10.2018"), DateTime.Parse("31.12.2018"), "ACTIVE"));
            list.Add(new Assignment("NAME3", DateTime.Parse("26.10.2018"), DateTime.Parse("15.12.2018"), "ACTIVE"));
            list.Add(new Assignment("NAME4", DateTime.Parse("20.10.2018"), DateTime.Parse("21.12.2018"), "INACTIVE"));
            list.Add(new Assignment("NAME5", DateTime.Parse("23.10.2018"), DateTime.Parse("15.1.2019"), "INACTIVE"));
            List<User> allUsers = new List<User>();

            User currentUser = new User();
            User leaderUser = new User();
            leaderUser.username = "John Doe";
            Group skupina = new Group();
            skupina.Leader = leaderUser;
            skupina.Name = "GROUP1";
            skupina.Workers = new List<User>();
            skupina.Workers.Add(currentUser);
            currentUser.Assignments = list;
            currentUser.username = "WorkerUser1";
            currentUser.privilege = "Worker";
            //currentUser.Groups = new List<Group>();
            currentUser.Groups.Add(skupina);
            allUsers.Add(currentUser);
            allUsers.Add(leaderUser);
            if (currentUser.Role == "Worker")
            {
                return View(currentUser);
            }
            else if (currentUser.Role == "Manager")
            {
                return View(currentUser);
            }
            else
            {
                return View(currentUser);
            }*/
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}