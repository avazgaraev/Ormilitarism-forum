using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ormilitarism.Models
{
    public class post
    {
        [Key]
        public int postid { get; set; }

        //[Column(TypeName = "nvarhcar")]
        //[MaxLength(30)]
        //[Required]
        //public string postbasliq { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength()]
        [Required]
        public string postmezmun { get; set; }

        public int postlikecount { get; set; }

        public int postfavcount{ get; set; }

        public DateTime posttime { get; set; }

        public int Customerid { get; set; }
        public virtual customer Customer { get; set; }
        
        public virtual ICollection<postimgs> Postimgs { get; set; }
        public virtual ICollection<like> Likes{ get; set; }
        public virtual ICollection<favorit> Favorits{ get; set; }


        public int Titleid { get; set; }
        public virtual title Title { get; set; }
    }
}