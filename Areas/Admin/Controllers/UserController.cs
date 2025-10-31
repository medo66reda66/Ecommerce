using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.SUPER_ADMIN_ROLE)]
    public class UserController : Controller
    {
        private readonly UserManager<Appliccationusr> _userManager;

        public UserController(UserManager<Appliccationusr> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            return View(_userManager.Users);
        }

        public async Task<IActionResult> LockUnLock(string Id)
        {
           var user = await _userManager.FindByIdAsync(Id);
            if (user == null) 
                return NotFound();

            if(await _userManager.IsInRoleAsync(user,DS.SUPER_ADMIN_ROLE))
            {
                TempData["error-Notification"] = "no super Admin lock";
                return RedirectToAction(nameof(Index)); 
            }

            user.LockoutEnabled = !user.LockoutEnabled;
            await _userManager.UpdateAsync(user);
            TempData["sucess-Notification"] = $"blocl {user.firstname} {user.lastname}";

            return RedirectToAction(nameof(Index));
        }


    }
}
