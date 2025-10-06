
using Ecommerce.DateAcsess.Entitytypeconficration;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DateAcsess
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Categores> Categores { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductColors> ProductColors { get; set; }
        public DbSet<ProductSubimgs> ProductSubimgs { get; set; }
        public object Categorys { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial catalog=ECommerce1; Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProuductColerEntitytypeconfigration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
