using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNASTWebsite.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
namespace VNASTWebsite.Controllers
{
    [Authorize]
    [HandleError]
    public class HomeController : Controller
    {
        public static string sortBy { get; set; }
        public ActionResult Index()
        {
            try
            {
                string get_currentUser = AccountController.apiRequestController.RequestGet("me");
                var currentUser = JsonConvert.DeserializeObject<User>(get_currentUser);

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
                    if (sortBy != null)
                    {
                        switch (sortBy)
                        {
                            case "priority":
                                currentUser.tasks = currentUser.tasks.OrderByDescending(x => x.priority.Length).ThenBy(x => x.priority == null).ThenBy(x => x.priority).ToList();
                                break;
                            case "status":
                                currentUser.tasks = currentUser.tasks.OrderBy(x => x.status[0]).ToList();
                                break;
                            case "time_limit":
                                currentUser.tasks = currentUser.tasks.OrderBy(x => x.time_limit).ToList();
                                break;
                            case "name":
                                currentUser.tasks = currentUser.tasks.OrderBy(x => x.name).ToList();
                                break;
                            default: break;
                        }
                    }
                    
                    return View(currentUser);
                }
                else if (currentUser.privilege[0] == "manager")
                {
                    //   string get_userTasks = AccountController.apiRequestController.RequestGet("tasks/get/mytasks");
                    //   var userTasks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Assignment>>(get_userTasks);
                    string get_userGroupManagerOf = AccountController.apiRequestController.RequestGet("groups/get/managerof");
                    var userGroups = JsonConvert.DeserializeObject<List<Group>>(get_userGroupManagerOf);
                    //   currentUser.Assignments = userTasks;
                    currentUser.Groups = userGroups;

                    string get_ManagerTasks = AccountController.apiRequestController.RequestGet("tasks/get/managedtasks");
                    var ManagerTasks = JsonConvert.DeserializeObject<List<Assignment>>(get_ManagerTasks);

                    currentUser.tasks = ManagerTasks;
                    switch (sortBy)
                    {
                        case "priority":
                            currentUser.tasks = currentUser.tasks.OrderByDescending(x => x.priority.Length).ThenBy(x => x.priority == null).ThenBy(x => x.priority).ToList();
                            break;
                        case "status":
                            currentUser.tasks = currentUser.tasks.OrderBy(x => x.status[0]).ToList();
                            break;
                        case "time_limit":
                            currentUser.tasks = currentUser.tasks.OrderBy(x => x.time_limit).ToList();
                            break;
                        case "name":
                            currentUser.tasks = currentUser.tasks.OrderBy(x => x.name).ToList();
                            break;
                    }

                    List<User> managerWorkers = new List<User>();
                    foreach (var item in currentUser.tasks)
                    {
                        string get_User = AccountController.apiRequestController.RequestGet("users/" + item.assigned_to_worker);
                        
                       
                        try
                        {
                            var user = JsonConvert.DeserializeObject<User>(get_User.TrimStart('[').TrimEnd(']'));

                            item.assigned_to_workerName = user.username;
                        }
                        catch { }
                    }
                    foreach (var item in userGroups)
                    {
                        item.Workers = new List<User>();
                        foreach (var item1 in item.workers)
                        {
                            string get_User = AccountController.apiRequestController.RequestGet("users/" + item1);
                            var user = JsonConvert.DeserializeObject<User>(get_User.TrimStart('[').TrimEnd(']'));
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

        public ActionResult GroupChat(string id)
        {
            string get_my_group_chats = AccountController.apiRequestController.RequestGet("groups/" + id + "/chats");
            var chats = JsonConvert.DeserializeObject<List<Chat>>(get_my_group_chats);


            List<Chat> new_my_chats = new List<Chat>();
            foreach (Chat chat in chats)
            {
                List<string> created_by_names = new List<string>();
                foreach (string id2 in chat.participants)
                {
                    string get_user = AccountController.apiRequestController.RequestGet("users/" + id2);
                    var user = JsonConvert.DeserializeObject<User>(get_user.TrimStart('[').TrimEnd(']'));
                    created_by_names.Add(user.username);
                }
                chat.participants = created_by_names;
                new_my_chats.Add(chat);
            }
            ViewBag.List = new_my_chats;
            return View();
        }

        // GET: Display messages in group chat
        public ActionResult ShowGroupChat(string id, string id2)
        {
            try
            {
                string get_messages = AccountController.apiRequestController.RequestGet("groups/" + id +"/chats/" + id2);
                var messages = JsonConvert.DeserializeObject<List<Message>>(get_messages);
                List<Message> updated_messages = new List<Message>();
                foreach (Message message in messages)
                {
                    string created_by_id = message.created_by;
                    string get_user = AccountController.apiRequestController.RequestGet("users/" + created_by_id);
                    var user = JsonConvert.DeserializeObject<User>(get_user.TrimStart('[').TrimEnd(']'));
                    message.created_by = user.username;
                    updated_messages.Add(message);
                }
                ViewBag.Message = "Chat name";
                Response.AddHeader("Refresh", "5");
                return View("Messages", messages);
            }
            catch { return View(); }
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
            try
            {
                string get_currentUser = AccountController.apiRequestController.RequestGet("me");
                var currentUser = JsonConvert.DeserializeObject<User>(get_currentUser);

                string get_my_chats = AccountController.apiRequestController.RequestGet("chats/get/memberin");
                var my_chats = JsonConvert.DeserializeObject<List<Chat>>(get_my_chats);
                currentUser.my_chats = my_chats;

                string get_userGroup = AccountController.apiRequestController.RequestGet("groups/get/memberin");
                var userGroups = JsonConvert.DeserializeObject<List<Group>>(get_userGroup);
                currentUser.Groups = userGroups;

                return View(currentUser);
            }
            catch {
                return View();
            }
        }

        public ActionResult Evaluate(string id)
        {
            try
            {
                string get_assignments = AccountController.apiRequestController.RequestGet("tasks");
                var assignments = JsonConvert.DeserializeObject<List<Models.Assignment>>(get_assignments);
                var assignmentToEdit = assignments.Where(s => s._id == id).FirstOrDefault();
                string get_users = AccountController.apiRequestController.RequestGet("users");
                var users = JsonConvert.DeserializeObject<List<User>>(get_users);
                assignmentToEdit.potentialWorkers = users;
                return View(assignmentToEdit);
            }
            catch { return View("Error"); }
        }

        [HttpPost]
        public ActionResult Evaluate(Models.Assignment assignment, IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null) { 
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/"), fileName);
                        file.SaveAs(path);
                        WebClient myWebClient = new WebClient();
                        //myWebClient.Headers.Add("x-access-token",APIController.userToken);
                       // myWebClient.Headers.Add("User-Agent: Other");
                       // myWebClient.UseDefaultCredentials = true;
             
                        byte[] responseArray = myWebClient.UploadFile("http://13.80.47.169:3000/tasks/" + assignment._id + "/files", "POST", path);
                    }
                    }
                }
            }


            if (assignment.filename != null)
            {
                Document d = new Document();
                d.originalname = assignment.filename;
                d.filename = assignment.filename;
                d.path = d.filename;

                string mimeType = MimeMapping.GetMimeMapping(assignment.filename);
                d.mimetype = mimeType;
                if (assignment.documents == null)
                {
                    assignment.documents = new List<Document>();
                }
                // assignment.documents.Add(d);


                WebClient myWebClient = new WebClient();



                // Console.WriteLine("Uploading {0} to {1} ...", d.originalname, uriString);
                // Upload the file to the URL using the HTTP 1.0 POST.

             //   byte[] responseArray = myWebClient.UploadFile("/tasks/" + assignment._id + "/files", "POST", d.originalname);

            }
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
        public ActionResult Accept(string id)
        {

            string get_assignments = AccountController.apiRequestController.RequestGet("tasks");
            var assignments = JsonConvert.DeserializeObject<List<Models.Assignment>>(get_assignments);
            var assignmentToEdit = assignments.Where(s => s._id == id).FirstOrDefault();
            string get_users = AccountController.apiRequestController.RequestGet("users");
            var users = JsonConvert.DeserializeObject<List<User>>(get_users);
            assignmentToEdit.potentialWorkers = users;
            assignmentToEdit.status[0] = "completed";
            bool update_data = AccountController.apiRequestController.EditAssignmentRequest(assignmentToEdit);
            if (update_data)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }

        }
        public ActionResult Decline(string id)
        {

            string get_assignments = AccountController.apiRequestController.RequestGet("tasks");
            var assignments = JsonConvert.DeserializeObject<List<Models.Assignment>>(get_assignments);
            var assignmentToEdit = assignments.Where(s => s._id == id).FirstOrDefault();
            string get_users = AccountController.apiRequestController.RequestGet("users");
            var users = JsonConvert.DeserializeObject<List<User>>(get_users);
            assignmentToEdit.potentialWorkers = users;
            assignmentToEdit.status[0] = "ongoing";
            bool update_data = AccountController.apiRequestController.EditAssignmentRequest(assignmentToEdit);
            if (update_data)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
           
        }

        void Sort() { }

        public ActionResult SortByPriority(string id)
        {

            sortBy = "priority";
            return RedirectToAction("Index");
        }
        public ActionResult SortByStatus(string id)
        {

            sortBy = "status";
            return RedirectToAction("Index");
        }
            public ActionResult SortByName(string id)
        {

            sortBy = "name";
            return RedirectToAction("Index");
        }
        public ActionResult SortByTimeLimit(string id)
        {

            sortBy = "time_limit";
            return RedirectToAction("Index");
        }
        public RedirectResult Download(string taskid, string fileid, string path, string mime, string orgname)
        {
            try
            {
                string url= "http://13.80.47.169:3000/tasks/"+taskid+"/files/"+fileid;


                string remoteUri = url;
                string fileName = orgname;
                
                WebClient myWebClient = new WebClient();
               
                return new RedirectResult(url);

            
            }
            catch (IOException e) {// return View("Error");
            }
            
            return null;
        }

    }

}