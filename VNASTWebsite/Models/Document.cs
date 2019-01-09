using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VNASTWebsite.Models
{
    public class Document
    {
        
         public string _id { get; set; }
         public string mimetype { get; set; }
         public string originalname { get; set; }
         public string filename { get; set; }
         public string path { get; set; }
        
    }
}