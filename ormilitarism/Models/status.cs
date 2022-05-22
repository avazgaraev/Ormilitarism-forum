using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ormilitarism.Models
{
    public class status
    {
        [Key]
        public int statusid { get; set; }
        public string statusad { get; set; }
        public string likecount { get; set; }
        public string favcount { get; set; }
        public string postcount { get; set; }
        public string titlecount { get; set; }

        public virtual ICollection<customer> Customers { get; set; }
        public virtual ICollection<report> Reports{ get; set; }

    }
}