using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VNASTWebsite.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<Group> Groups { get; set; }
    }
}