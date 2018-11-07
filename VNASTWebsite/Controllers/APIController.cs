using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace VNASTWebsite.Controllers
{
    public class APIController : Controller
    {
        private string serverUrl = "http://23.97.243.166:3000/";
        private string userToken;

        // GET: API
        public ActionResult Index()
        {
            return View();
        }

        // POST: login request to API server
        public bool LoginRequest(string username, string password)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest
                .Create(serverUrl + "login");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json; charset=UTF-8";
            webRequest.Accept = "application/json";
            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                string requestBody = string.Format("{{\"username\":\"{0}\",\"password\":\"{1}\"}}", username, password);
                streamWriter.Write(requestBody);
                streamWriter.Close();
            }
            
            string jsonResponse = "";
            try
            {
                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        jsonResponse = sr.ReadToEnd();
                    }
                }

                JObject response = JObject.Parse(jsonResponse);
                userToken = (string)response["token"]; 
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        public bool RegisterRequest(string username, string password, string privilege = "", string email = "") // TODO handle privilege and email
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest
                .Create(serverUrl + "register");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json; charset=UTF-8";
            webRequest.Accept = "application/json";
            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                string requestBody = string.Format("{{\"username\":\"{0}\",\"password\":\"{1}\"}}", username, password);
                streamWriter.Write(requestBody);
                streamWriter.Close();
            }

            string jsonResponse = "";
            try
            {
                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        jsonResponse = sr.ReadToEnd();
                    }
                }

                JObject response = JObject.Parse(jsonResponse);
                userToken = (string)response["token"];

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        // GET: generic get method for API, parameter is page URI 
        public string RequestGet(string page)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest
                .Create(serverUrl + page);
            webRequest.Method = "GET";
            webRequest.Accept = "application/json";
            webRequest.Headers.Add("x-access-token", userToken);

            string jsonResponse = "";
            using (Stream s = webRequest.GetResponse().GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    jsonResponse = sr.ReadToEnd();
                }
            }
            return jsonResponse;
        }
    }
}