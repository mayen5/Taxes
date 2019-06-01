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

        public DbSet<PropertyType> PropertyTypes { get; set; }

        public System.Data.Entity.DbSet<Taxes.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<Taxes.Models.Municipality> Municipalities { get; set; }
    }
}