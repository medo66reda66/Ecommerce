using Ecommerce.viwemodel;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<Appliccationusr> _userManager;

        public ProfileController(UserManager<Appliccationusr> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            //var userVM = new ApplicationuserVM()
            //{
            //    FullName = $"{user.firstname} {user.lastname}",
            //    Email = user.Email,
            //    Address = user.adreeas,
            //    PhoneNumber = user.PhoneNumber,
            //};

            TypeAdapterConfig<Appliccationusr, ApplicationuserVM>
                .NewConfig()
                .Map(d => d.FullName, s => $"{s.firstname} {s.lastname}")
                .TwoWays();

            var userVM = user.Adapt<ApplicationuserVM>();

            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ApplicationuserVM applicationuserVM)
        {
           var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var name = applicationuserVM.FullName.Split(" ");
            user.PhoneNumber = applicationuserVM.PhoneNumber;
            user.Email = applicationuserVM.Email;
            user.adreeas = applicationuserVM.Address;
            user.firstname = name[0];
            user.lastname = name[1];

          var result =  await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(applicationuserVM);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Updatepassword(ApplicationuserVM applicationuserVM)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound();

            if (applicationuserVM.CurrentPassword is not null && applicationuserVM.NewPassword is not null)
                return RedirectToAction(nameof(Index));


          var result =  await _userManager.ChangePasswordAsync(user, applicationuserVM.CurrentPassword,applicationuserVM.NewPassword);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "invalid password");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
