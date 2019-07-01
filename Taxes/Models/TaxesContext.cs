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

            modelBuilder.Entity<Employee>()
                .HasOptional(x => x.Boss)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.BossId);
        }



        public DbSet<Department> Departments { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Municipality> Municipalities { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<Tax> Taxes { get; set; }

        public DbSet<Taxpayer> Taxpayers { get; set; }

        public DbSet<TaxProperty> TaxProperties { get; set; }

        public DbSet<Taxes.Models.Employee> Employees { get; set; }
    }
}