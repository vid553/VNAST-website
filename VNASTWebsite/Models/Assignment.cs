﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.IO;
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
        public List<Document> documents { get; set; }
        public string created_date { get; set; }
        public string _id { get; set; }

        [Required]
        public string name { get; set; }
        [Required]
        public string description { get; set; }

        /// <summary>
    
        /// </summary>
//public DateTime Created_date { get; set; }
        public int __v { get; set; }
        public string assigned_to_worker { get; set; }
        public string assigned_to_workerName { get; set; }
        public string created_by { get; set; }
        public string assigned_to_group { get; set; }
        public DateTime? time_limit { get; set; }
        public List<User> potentialWorkers { get; set; }
        public string filename { get; set; }
    }
}