using System.Diagnostics;
using Ecommerce.Models;
using Ecommerce.viwemodel;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDBContext db = new ApplicationDBContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var product= db.Products.AsEnumerable();
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public ViewResult Infoperson()
        {
             List<Person> people = new List<Person>();
            
                people.AddRange( new()
                    { 
                        Id=1,
                        FirstName="Mahmoud",
                        LastName="Zahra",
                        Age=20,
                        Email="medo@gmail.com" 
                    },new()
                    {
                        Id=2, 
                        FirstName="Ali", 
                        LastName="Ahmed",
                        Age=22, 
                        Email="ALI@123" 
                    });
            var pepolINDB = people.AsQueryable();
            var totalpeople = people.Count;

             return View(new personVM
            {
                 Pepoles = pepolINDB.ToList(),

                 totalpeople = totalpeople
            });
        }
                
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
