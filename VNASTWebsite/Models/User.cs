using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VNASTWebsite.Models
{
   /* public class User
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<Group> Groups { get; set; }
    }*/
    public class User
    {
        public List<string> privilege { get; set; }
        public string _id { get; set; }
        public string username { get; set; }
        public string email { get; set; }

        public List<User> workers { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<Group> Groups { get; set; }
    }
  
}