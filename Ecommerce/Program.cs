using Ecommerce.Repository;
using Ecommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddScoped<IRepository<Categores>, Repository<Categores>>();
            builder.Services.AddScoped<IRepository<Brands>, Repository<Brands>>();
            builder.Services.AddScoped<IRepository<ProductSubimgs>, Repository<ProductSubimgs>>();
            builder.Services.AddScoped<IRepository<ProductColors>, Repository<ProductColors>>();
            builder.Services.AddScoped<productIRepositry,producrRepository>();
            builder.Services.AddScoped<productcolerIRepositry,productcolerRepository>();
            
            var app = builder.Build();

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
