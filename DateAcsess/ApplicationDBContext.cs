
using Ecommerce.DateAcsess.Entitytypeconficration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ecommerce.viwemodel;

namespace Ecommerce.DateAcsess
{
    public class ApplicationDBContext : IdentityDbContext<Appliccationusr>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContext): base(dbContext) 
        {

        }
        public DbSet<Categores> Categores { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductColors> ProductColors { get; set; }
        public DbSet<ProductSubimgs> ProductSubimgs { get; set; }
        public DbSet<ApplicationUserOtp> ApplicationUserOtps { get; set; }
     

        //public ApplicationDBContext()
        //{

        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial catalog=ECommerce1; Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProuductColerEntitytypeconfigration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        //public DbSet<Ecommerce.viwemodel.VolidateOtpVM> VolidateOtpVM { get; set; } = default!;
        //public DbSet<Ecommerce.viwemodel.NewPasswordVM> NewPasswordVM { get; set; } = default!;
        //public DbSet<Ecommerce.viwemodel.RegisterVM> RegisterVM { get; set; } = default!;
        //public DbSet<Ecommerce.viwemodel.LoginVM> LoginVM { get; set; } = default!;
        //public DbSet<Ecommerce.viwemodel.ResendEmailConfirmVM> ResendEmailConfirmVM { get; set; } = default!;
        //public DbSet<Ecommerce.viwemodel.ForgetPasswordVM> ForgetPasswordVM { get; set; } = default!;
    }
}
