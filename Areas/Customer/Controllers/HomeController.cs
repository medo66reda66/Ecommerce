using System.Diagnostics;
using Ecommerce.Models;
using Ecommerce.viwemodel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDBContext db;//= new();
        public HomeController(ILogger<HomeController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            this.db = db;
        }

     
        public IActionResult Index(filterdataVM filterdataVM)
        {
            var product= db.Products.Include(e=>e.Category).Include(e=>e.Brand).AsQueryable();
           
            if(filterdataVM.name is not null)
            {
                product = product.Where(e => e.Name.Contains(filterdataVM.name));
                ViewBag.name = filterdataVM.name;
            }

            if(filterdataVM.minprice is not null)
            {
                product = product.Where(e => e.Price - e.Price * (e.Discount / 100) >= filterdataVM.minprice);
                ViewBag.minprice = filterdataVM.minprice;
            }

            if(filterdataVM.maxprice is not null)
            {
                product = product.Where(e => e.Price - e.Price * (e.Discount / 100) <= filterdataVM.maxprice);
                ViewBag.maxprice = filterdataVM.maxprice;
            }

            if(filterdataVM.catecoryid is not null)
            {
                product = product.Where(e => e.CategoryId == filterdataVM.catecoryid);
                ViewBag.catecoryid = filterdataVM.catecoryid;
            }

            if(filterdataVM.brandid is not null)
            {
                product = product.Where(e => e.BrandId == filterdataVM.brandid);
                ViewBag.brandid = filterdataVM.brandid;
            }

            if (filterdataVM.hot)
            {
                product = product.Where(e => e.Discount >= 50);
                ViewBag.hot = filterdataVM.hot;
            }
            var category = db.Categores;
            ViewBag.category = category.AsQueryable();
            var brand = db.Brands;
            ViewBag.brands = brand.AsQueryable();

            return View(product.AsNoTracking().AsEnumerable());
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        public ViewResult Infoperson()
        {
             List<Person> people = new List<Person>();
            
                //people.AddRange( new()
                //    { 
                //        Id=1,
                //        FirstName="Mahmoud",
                //        LastName="Zahra",
                //        Age=20,
                //        Email="medo@gmail.com" 
                //    },new()
                //    {
                //        Id=2, 
                //        FirstName="Ali", 
                //        LastName="Ahmed",
                //        Age=22, 
                //        Email="ALI@123" 
                //    });
            var pepolINDB = people.AsQueryable();
            var totalpeople = people.Count;

             return View(new personVM
            {
                 Pepoles = pepolINDB.ToList(),

                 totalpeople = totalpeople
            });
        }
                
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
