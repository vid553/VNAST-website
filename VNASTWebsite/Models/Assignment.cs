using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VNASTWebsite.Models
{
    public class Assignment
    {

       /* public List<User> Workers { get; set; }
        public User Manager { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime Deadline { get; set; }
        public int Importance { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Status { get; set; } //AKTIVNA, NEAKTIVNA
        public Assignment(string n, DateTime cd, DateTime dl,string s)
        {
            Name = n;
            CreationDate = cd;
            Deadline = dl;
            Status = s;
        }*/
          public string priority { get; set; }
          public List<string> status { get; set; }
          public List<object> documents { get; set; }
          public DateTime created_date { get; set; }
          public string _id { get; set; }
          public string name { get; set; }
          public string description { get; set; }
          public DateTime Created_date { get; set; }
          public int __v { get; set; }
          public string assigned_to_worker { get; set; }
          public string created_by { get; set; }
          public string assigned_to_group { get; set; }
          public DateTime? time_limit { get; set; }
        
    }
}