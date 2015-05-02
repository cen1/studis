using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace studis.Models
{
    public class TretjiPredmetnikModuliModel
    {
        
        

        [Required]
        [Display(Name = "Dodatni predmet")]
        public int izbirni { get; set; }
    }
}