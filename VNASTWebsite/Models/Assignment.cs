using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VNASTWebsite.Models
{
    public class Assignment
    {

        public List<User> Workers { get; set; }
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
        }
    }
}