using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Taxes.Models
{
    public class TaxesContext : DbContext
    {
        public TaxesContext() : base("DefaultConnection")
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public System.Data.Entity.DbSet<Taxes.Models.PropertyType> PropertyTypes { get; set; }
    }
}