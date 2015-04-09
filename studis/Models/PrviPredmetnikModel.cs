using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace studis.Models
{
    public class PrviPredmetnikModel
    {

        [Display(Name = "vpisnilist")]
        public int vlid { get; set; }


        public IQueryable<predmet> predmeti { get; set; }
    }
}