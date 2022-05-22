using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ormilitarism.Models
{
    public class report
    {
        [Key]
        public int reportid { get; set; }
        public string reportmezmun{ get; set; }
        public int? statusid { get; set; }
        public virtual status Status { get; set; }
        public DateTime reportdate { get; set; }
        public DateTime reportdeadline { get; set; }
        public bool reportavailable{ get; set; }
    }
}