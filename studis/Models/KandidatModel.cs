using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace studis.Models
{
    public class KandidatModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "{0} mora biti dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Ime")]
        public string ime { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "{0} mora biti dolg vsaj {2} znakov.", MinimumLength = 1)]
        [Display(Name = "Priimek")]
        public string priimek { get; set; }

        [Required]
        [Display(Name = "Študijski program")]
        public int studijskiProgram { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(60, ErrorMessage = "{0} ne sme biti daljši od {2} znakov.")]
        [Display(Name = "Email naslov")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Vnešen vpisni list")]
        public Boolean vnesen { get; set; }
    }
}