using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ormilitarism.Models
{
    public class favorit
    {
        [Key]
        public int favid { get; set; }
        public int postid { get; set; }
        public virtual post Post { get; set; }
        public int custid { get; set; }
        public virtual customer customer { get; set; }
        public string custname { get; set; }
        public DateTime favdate { get; set; }
    }
}