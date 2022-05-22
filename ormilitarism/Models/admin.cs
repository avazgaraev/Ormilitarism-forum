using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ormilitarism.Models
{
    public class admin
    {
        [Key]
        public int adminid { get; set; }
        public string adminname { get; set; }

        [Column(TypeName = "nvarchar")]
        [MinLength(8, ErrorMessage = "Please enter minimum 8 characters.")]
        [StringLength(24, ErrorMessage = "Please enter max 24 characters.")]
        [Required(ErrorMessage = "Please enter password.")]
        public string adminpass { get; set; }
        [Required]
        [Compare("adminpass", ErrorMessage = "parolla eyni poxu yaz")]
        public string confirmpass { get; set; }

        public string adminimg { get; set; }
        public DateTime adminregister { get; set; }
    }
}