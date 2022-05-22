using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ormilitarism.Models
{
    public class sorgu
    {
        [Key]
        public int sorguid { get; set; }
        public string sorguad{ get; set; }

        [ForeignKey("sorguid")]
        public virtual ICollection<title> Titles { get; set; }
    }
}