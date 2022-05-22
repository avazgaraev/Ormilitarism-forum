using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ormilitarism.Models
{
    public class title
    {
        [Key]
        public int titleid { get; set; }

        [MinLength(3, ErrorMessage = "Please enter minimum 3 characters.")]
        [Required(ErrorMessage = "Please enter title name.")]
        //[Index(IsUnique = true)]
        public string titlename { get; set; }
        public int postcount { get; set; }
        public DateTime titleregister { get; set; }

        public virtual ICollection<post> Posts { get; set; }

        public int? sorguid { get; set; }
        [ForeignKey("sorguid")]
        public virtual sorgu Sorgu { get; set; }

        public int? customerid { get; set; }
        [ForeignKey("customerid")]
        public virtual customer Customer { get; set; }
    }
}