using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ormilitarism.Models
{
    public class postimgs
    {
        [Key]
        public int imgid { get; set; }
        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public string imgname { get; set; }

        public string imgurl { get; set; }
        public int postid { get; set; }
        public virtual post Post { get; set; }
    }
}