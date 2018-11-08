using Newtonsoft.Json;
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

        // POST: Register new user
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

        public bool AddAssignmentRequest(Models.Assignment assignment)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest
               .Create(serverUrl + "tasks");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json; charset=UTF-8";
            webRequest.Accept = "application/json";
            webRequest.Headers.Add("x-access-token", userToken);
            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                string requestBody = JsonConvert.SerializeObject(assignment, serializerSettings);
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
            try
            {
                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        jsonResponse = sr.ReadToEnd();
                    }
                }
            }
            catch(Exception ex)
            {
                return "";
            }
            return jsonResponse;
        }

        // PUT to edit user, we check resulting user from server against parameter user
        public bool EditUserRequest(Models.User user)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest
                .Create(serverUrl + "users/" + user._id);
            webRequest.Method = "PUT";
            webRequest.Accept = "application/json";
            webRequest.ContentType = "application/json; charset=UTF-8";
            webRequest.Headers.Add("x-access-token", userToken);
            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                user._id = null;
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                string requestBody = JsonConvert.SerializeObject(user, serializerSettings);
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

                //JObject response = JObject.Parse(jsonResponse);
                Models.User api_user = JsonConvert.DeserializeObject<Models.User>(jsonResponse);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        // PUT to edit assignment, we check resulting assignment from server against parameter
        public bool EditAssignmentRequest(Models.Assignment assignment)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest
                .Create(serverUrl + "tasks/" + assignment._id);
            webRequest.Method = "PUT";
            webRequest.Accept = "application/json";
            webRequest.ContentType = "application/json; charset=UTF-8";
            webRequest.Headers.Add("x-access-token", userToken);
            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                assignment._id = null;
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                string requestBody = JsonConvert.SerializeObject(assignment, serializerSettings);
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
                Models.Assignment api_assignment = JsonConvert.DeserializeObject<Models.Assignment>(jsonResponse);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        // DELETE: generic delete method for API, parameter is page URI
        public bool RequestDelete(string page)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest
                .Create(serverUrl + page);
            webRequest.Method = "DELETE";
            webRequest.Headers.Add("x-access-token", userToken);

            try
            {
                string jsonResponse = "";
                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        jsonResponse = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
    }
}