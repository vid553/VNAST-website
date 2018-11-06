using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace VNASTWebsite.Controllers
{
    public class APIController : Controller
    {
        // GET: API
        public ActionResult Index()
        {
            return View();
        }

        public string LoginRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest
                .Create("http://23.97.243.166:3000/login");
            webRequest.KeepAlive = false;
            webRequest.Method = "POST";

            webRequest.ContentType = "application/json; charset=UTF-8";
            webRequest.Accept = "application/json";
            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                string json = " { 'username': 'admin', 'password': 'admin' } ";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
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