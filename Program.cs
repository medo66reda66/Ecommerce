using Ecommerce.Config;
using Ecommerce.Repository;
using Ecommerce.Repository.IRepository;
using Ecommerce.Utilities;
using Ecommerce.Utilities.DBinitializer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ecommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDBContext>(option =>
            {
                //option.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["default"]);
                //option.UseSqlServer(builder.Configuration["ConnectionStrings : default"]);
                option.UseSqlServer(builder.Configuration.GetConnectionString("default"));


            });
            builder.Services.AddIdentity<Appliccationusr, IdentityRole>(Option =>
            {
                Option.Password.RequiredLength = 6;
                Option.Password.RequireLowercase = false;
                Option.Password.RequireUppercase = false;
                Option.Password.RequireNonAlphanumeric = false;
                Option.User.RequireUniqueEmail = true;
                Option.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<ApplicationDBContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddTransient<IEmailSender,EmailSender>();
            builder.Services.AddScoped<IRepository<Categores>, Repository<Categores>>();
            builder.Services.AddScoped<IRepository<Brands>, Repository<Brands>>();
            builder.Services.AddScoped<IRepository<ProductSubimgs>, Repository<ProductSubimgs>>();
            builder.Services.AddScoped<IRepository<ProductColors>, Repository<ProductColors>>();
            builder.Services.AddScoped<IRepository<ApplicationUserOtp>, Repository<ApplicationUserOtp>>();
            builder.Services.AddScoped<productIRepositry,producrRepository>();
            builder.Services.AddScoped<productcolerIRepositry,productcolerRepository>();
            builder.Services.AddScoped<IBDinitializer, DBinitializer>();
            
            builder.Services.RegisterMapsterConfig();

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IBDinitializer>();
            service!.Initializ();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
