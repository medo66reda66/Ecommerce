using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =$"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE},{DS.EMPLOYEE_ROLE}")]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NOTfoundnwe()
        {
            return View();
        }
    }
}
