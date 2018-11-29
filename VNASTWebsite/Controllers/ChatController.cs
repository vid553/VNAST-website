﻿using System;
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
                currentUser.my_chats = my_chats;

                if (currentUser.privilege[0] == "admin" || currentUser.privilege[0] == "manager")
                {
                    // display also chats from other users
                    string get_chats = AccountController.apiRequestController.RequestGet("chats");
                    var all_chats = JsonConvert.DeserializeObject<List<Chat>>(get_chats);
                    currentUser.all_chats = all_chats;
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
            string get_messages = AccountController.apiRequestController.RequestGet("chats/" + id);
            var messages = JsonConvert.DeserializeObject<List<Message>>(get_messages);

            ViewBag.Message = "Chat name";
            return View("Messages", messages);
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