using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VNASTWebsite.Models
{
    public class Message
    {
        public string _id { get; set; }
        public string created_date { get; set; }
        public string content { get; set; }
        public string created_by { get; set; }
    }
}