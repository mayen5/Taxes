namespace Taxes.Models
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class TaxesContext : DbContext
    {
        public TaxesContext() : base("DefaultConnection")
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Municipality> Municipalities { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Taxpayer> TaxPaers { get; set; }
    }
}