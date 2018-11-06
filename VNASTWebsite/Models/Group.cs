using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VNASTWebsite.Models
{
    public class Group
    {
        public string Name { get; set; }
        public User Leader { get; set; }
        public List<User> Workers { get; set; }
    }
}