using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNASTWebsite.Models;
using Newtonsoft.Json;

namespace VNASTWebsite.Controllers
{
    [Authorize]
    [HandleError]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            //     APIController api = AccountController.apiRequestController;
            //   string json = api.RequestGet("/me");

            try
            {
                string get_currentUser = AccountController.apiRequestController.RequestGet("me");
                var currentUser = JsonConvert.DeserializeObject<User>(get_currentUser);

                //if (post_login)
                //{
                //    string get_user = apiRequestController.RequestGet("me");
                //}
                int c = 0;
                if (currentUser.privilege[0] == "admin")
                {
                    string get_users = AccountController.apiRequestController.RequestGet("users");
                    var users = JsonConvert.DeserializeObject<List<User>>(get_users);
                    currentUser.workers = users;
                    return View(currentUser);
                }
                else if (currentUser.privilege[0] == "worker")
                {
                    string get_userTasks = AccountController.apiRequestController.RequestGet("tasks/get/mytasks");
                    var userTasks = JsonConvert.DeserializeObject<List<Assignment>>(get_userTasks);
                    string get_userGroup = AccountController.apiRequestController.RequestGet("groups/get/memberin");
                    var userGroups = JsonConvert.DeserializeObject<List<Group>>(get_userGroup);
                    currentUser.tasks = userTasks;
                    currentUser.Groups = userGroups;
                    return View(currentUser);
                }
                else if (currentUser.privilege[0] == "manager")
                {
                    //   string get_userTasks = AccountController.apiRequestController.RequestGet("tasks/get/mytasks");
                    //   var userTasks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Assignment>>(get_userTasks);
                    string get_userGroupManagerOf = AccountController.apiRequestController.RequestGet("groups/get/managerof");
                    var userGroups = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Group>>(get_userGroupManagerOf);
                    //   currentUser.Assignments = userTasks;
                    currentUser.Groups = userGroups;

                    string get_ManagerTasks = AccountController.apiRequestController.RequestGet("tasks/get/managedtasks");
                    var ManagerTasks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Assignment>>(get_ManagerTasks);
                    currentUser.tasks = ManagerTasks;

                    List<User> managerWorkers = new List<User>();
                    foreach (var item in currentUser.tasks)
                    {
                        string get_User = AccountController.apiRequestController.RequestGet("users/" + item.assigned_to_worker);
                        var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(get_User.TrimStart('[').TrimEnd(']'));

                        item.assigned_to_workerName = user.username;
                    }
                    foreach (var item in userGroups)
                    {
                        item.Workers = new List<User>();
                        foreach (var item1 in item.workers)
                        {
                            string get_User = AccountController.apiRequestController.RequestGet("users/" + item1);
                            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(get_User.TrimStart('[').TrimEnd(']'));
                            if (user != null)
                            {
                                item.Workers.Add(user);
                                if (!managerWorkers.Any(x => x._id == user._id))
                                {
                                    
                                    managerWorkers.Add(user);

                                }
                            }
                        }



                    }
                    currentUser.workers = managerWorkers;
                    return View(currentUser);
                }
                return View(currentUser);
            }
            catch { }
            return View();
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

        public ActionResult EditUser(string id)
        {
            try
            {
                string get_currentUser = AccountController.apiRequestController.RequestGet("me");
                var currentUser = JsonConvert.DeserializeObject<User>(get_currentUser);
                string get_users = AccountController.apiRequestController.RequestGet("users");
                var users = JsonConvert.DeserializeObject<List<User>>(get_users);
                currentUser.workers = users;
                var userToEdit = currentUser.workers.Where(s => s._id == id).FirstOrDefault();
                return View(userToEdit);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            bool update_data = AccountController.apiRequestController.EditUserRequest(user);
            if (update_data)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult DeleteUser(string id)
        {
            bool delete_user = AccountController.apiRequestController.RequestDelete("users/" + id);
            if (delete_user)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }
        public ActionResult Assign(string id)
        {

            string get_assignments = AccountController.apiRequestController.RequestGet("tasks");
            var assignments = JsonConvert.DeserializeObject<List<Models.Assignment>>(get_assignments);
            var assignmentToEdit = assignments.Where(s => s._id == id).FirstOrDefault();
            string get_users = AccountController.apiRequestController.RequestGet("users");
            var users = JsonConvert.DeserializeObject<List<User>>(get_users);
            assignmentToEdit.potentialWorkers = users;
            return View(assignmentToEdit);
        }

        [HttpPost]
        public ActionResult Assign(Models.Assignment assignment)
        {
            bool update_data = AccountController.apiRequestController.EditAssignmentRequest(assignment);
            if (update_data)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult UserHomePage()
        {
            ViewBag.Message = "User Home Page";
            return View();
        }

        public ActionResult Chat()
        {
            ViewBag.Message = "Chat";
            return View();
        }
        public ActionResult Evaluate(string id)
        {

            string get_assignments = AccountController.apiRequestController.RequestGet("tasks");
            var assignments = JsonConvert.DeserializeObject<List<Models.Assignment>>(get_assignments);
            var assignmentToEdit = assignments.Where(s => s._id == id).FirstOrDefault();
            string get_users = AccountController.apiRequestController.RequestGet("users");
            var users = JsonConvert.DeserializeObject<List<User>>(get_users);
            assignmentToEdit.potentialWorkers = users;
            return View(assignmentToEdit);
        }

        [HttpPost]
        public ActionResult Evaluate(Models.Assignment assignment)
        {
            bool update_data = AccountController.apiRequestController.EditAssignmentRequest(assignment);
            if (update_data)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }
    }
}