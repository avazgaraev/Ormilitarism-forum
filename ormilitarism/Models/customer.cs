using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ormilitarism.Models
{
    public class customer
    {
        [Key]
        public int customerid { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(24, ErrorMessage = "Please enter max 24 characters.")]
        [MinLength(3, ErrorMessage = "Please enter minimum 3 characters.")]
        [Required(ErrorMessage = "Please enter name.")]
        [Index(IsUnique = true)]
        public string customername { get; set; }

        [Column(TypeName = "nvarchar")]
        [MinLength(8, ErrorMessage = "Please enter minimum 8 characters.")]
        [StringLength(24, ErrorMessage = "Please enter max 24 characters.")]
        [Required(ErrorMessage = "Please enter password.")] 
        public string customerpass { get; set; }

        [Required]
        [Compare("customerpass", ErrorMessage ="parolla eyni poxu yaz")]
        public string confirmpass { get; set; }


        public int postcount { get; set; }

        public int likecount { get; set; }
        public int favcount { get; set; }
        
        public int titlecount { get; set; }

        public int statusid { get; set; }
        public virtual status Status { get; set; }

        public string customerimg { get; set; }

        public DateTime registertime{ get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (customername.Length > 20 || customername.Length < 3)
                yield return new ValidationResult("Adə ən az 3, ən çox 20 karakter yaz ginən", new string[] { "customername" });
            if (customername.Length < 6)
                yield return new ValidationResult("Adə ən az 6 karakter yaz ginən", new string[] { "customerpass" });
        }

        public virtual ICollection<post> Posts { get; set; }

        [ForeignKey("customerid")]
        public virtual ICollection<title> Titles { get; set; }
        [ForeignKey("customerid")]
        public virtual ICollection<like> Likes { get; set; }
        public virtual ICollection<favorit> Favorits{ get; set; }

    }
}