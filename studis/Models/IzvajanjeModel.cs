using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace studis.Models
{
    public class IzvajanjeModel
    {

        [Required(ErrorMessage = "Obvezno izbrati.")]
        [Display(Name = "Predmet")]
        public int predmet { get; set; }

        [Required(ErrorMessage = "Obvezno izbrati.")]
        [Display(Name = "Profesor1")]
        public int profesor1 { get; set; }

        [Display(Name = "Profesor2")]
        public int profesor2 { get; set; }

        [Display(Name = "Profesor3")]
        public int profesor3 { get; set; }
    }
}