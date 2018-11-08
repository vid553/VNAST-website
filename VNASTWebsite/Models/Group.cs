using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VNASTWebsite.Models
{
    public class Group
    {
        [Required]
        public List<User> Workers { get; set; }
        public List<string> workers { get; set; }
        public string _id { get; set; }
        public string name { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public int __v { get; set; }
    }
}