//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace studis.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sifrant_studijskoletoprvegavpisa
    {
        public sifrant_studijskoletoprvegavpisa()
        {
            this.vpis = new HashSet<vpi>();
        }
    
        public int id { get; set; }
        public string naziv { get; set; }
    
        public virtual ICollection<vpi> vpis { get; set; }
    }
}
