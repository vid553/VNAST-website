using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VNASTWebsite.Models
{
    public class Chat
    {
        public List<string> participants { get; set; }
        public List<string> messages_ids { get; set; }
        public string _id { get; set; }
        public string created_date { get; set; }
        public string assigned_to_group { get; set; }
    }
}