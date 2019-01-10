using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNASTWebsite.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace VNASTWebsite.Controllers
{
    [Authorize]
    [HandleError]
    public class ChatController : Controller
    { 
        // GET: Chat - display chats
        public ActionResult Chat()
        {
            try
            {
                string get_currentUser = AccountController.apiRequestController.RequestGet("me");
                var currentUser = JsonConvert.DeserializeObject<User>(get_currentUser);

                string get_my_chats = AccountController.apiRequestController.RequestGet("chats/get/memberin");
                var my_chats = JsonConvert.DeserializeObject<List<Chat>>(get_my_chats);
                List<Chat> new_my_chats = new List<Chat>();
                foreach (Chat chat in my_chats)
                {
                    List<string> created_by_names = new List<string>();
                    foreach (string id in chat.participants)
                    {
                        string get_user = AccountController.apiRequestController.RequestGet("users/" + id);
                        var user = JsonConvert.DeserializeObject<User>(get_user.TrimStart('[').TrimEnd(']'));
                        created_by_names.Add(user.username);
                    }
                    chat.participants = created_by_names;
                    new_my_chats.Add(chat);
                }
                currentUser.my_chats = my_chats;

                // display also chats from other users
                if (currentUser.privilege[0] == "admin" || currentUser.privilege[0] == "manager")
                {
                    string get_chats = AccountController.apiRequestController.RequestGet("chats");
                    var all_chats = JsonConvert.DeserializeObject<List<Chat>>(get_chats);
                    List<Chat> new_all_chats = new List<Chat>();
                    foreach (Chat chat in all_chats)
                    {
                        List<string> created_by_names = new List<string>();
                        foreach (string id in chat.participants)
                        {
                            string get_user = AccountController.apiRequestController.RequestGet("users/" + id);
                            var user = JsonConvert.DeserializeObject<User>(get_user.TrimStart('[').TrimEnd(']'));
                            created_by_names.Add(user.username);
                        }
                        chat.participants = created_by_names;
                        new_all_chats.Add(chat);
                    }
                    currentUser.all_chats = new_all_chats;
                }
                else
                {
                    currentUser.all_chats = null;
                }

                ViewBag.Message = "Chats";
                return View(currentUser);
            }
            catch { return View(); }
        }

        // GET: Display messages in chat
        public ActionResult ShowChat(string id)
        {
            try
            {
                string get_messages = AccountController.apiRequestController.RequestGet("chats/" + id);
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

        public ActionResult CreateChat(string user_id)
        {
            if (user_id.Length != 0)
            {
                bool chat_create = AccountController.apiRequestController.CreateChat(user_id);
                if (chat_create)
                {
                    return RedirectToAction("Chat");
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
        }

        [HttpPost]
        public ActionResult SendMessage(string chat_id, string reply)
        {
            bool send = AccountController.apiRequestController.SendMessage(chat_id, reply);
            if (send)
            {
                return RedirectToAction("ShowChat", new { id = chat_id});
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult DeleteChat(string id)
        {
            bool delete = AccountController.apiRequestController.RequestDelete("chats/" + id);
            if (delete)
            {
                return RedirectToAction("Chat", new { id });
            }
            else
            {
                return View("Error");
            }
        }
    }
}