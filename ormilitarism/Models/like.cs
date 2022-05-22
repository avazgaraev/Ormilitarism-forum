using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ormilitarism.Models
{
    public class like
    {
        [Key]
        public int likeid { get; set; }
        public int postid { get; set; }
        public virtual post Post { get; set; }
        public string custname { get; set; }
        public int? customerid { get; set; }
        [ForeignKey("customerid")]
        public virtual customer Customer { get; set; }
        
    }
}