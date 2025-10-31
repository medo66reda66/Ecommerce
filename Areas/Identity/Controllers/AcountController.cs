using Ecommerce.Migrations;
using Ecommerce.Models;
using Ecommerce.Repository.IRepository;
using Ecommerce.viwemodel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AcountController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<Appliccationusr> _userManager;
        private readonly SignInManager<Appliccationusr> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<ApplicationUserOtp> _applicationUserOtpRepository;

        public AcountController(Microsoft.AspNetCore.Identity.UserManager<Appliccationusr> userManager , SignInManager<Appliccationusr> signInManager,
            IEmailSender emailSender, IRepository<ApplicationUserOtp> ApplicationUserOtpRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _applicationUserOtpRepository = ApplicationUserOtpRepository;
        }
        

        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var user = new Appliccationusr
            {
                firstname = registerVM.Firstname,
                lastname = registerVM.Lastname,
                PhoneNumber = registerVM.Phone,
                Email = registerVM.Email,
                UserName = registerVM.Username,
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("ConfirmEmail", "Acount", new { erea = "Identity", token, usrid = user.Id },Request.Scheme);

               await _emailSender.SendEmailAsync(registerVM.Email, "Ecomerse -_-_-..",
                $"<h1>Confirm Your email clicking <a href='{link}'>Here</a></h1>");

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Code);
                }
                return View(registerVM);
            }
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> ConfirmEmail(string token, string usrid)
        {
            var user = await _userManager.FindByIdAsync(usrid);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, " invalid user ");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                ModelState.AddModelError(string.Empty, "invalid token ");

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResendEmailConfirm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirm(ResendEmailConfirmVM resendEmailConfirmVM)
        {
            if (!ModelState.IsValid)
                return View(resendEmailConfirmVM);

            var user = await _userManager.FindByNameAsync(resendEmailConfirmVM.usernameOREmail) ?? await _userManager.FindByEmailAsync(resendEmailConfirmVM.usernameOREmail);


            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "invalid user Name OR Email ");
                return View(resendEmailConfirmVM);
            }

            if(user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Already confirm Email");
                return View(resendEmailConfirmVM);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("ConfirmEmail", "Acount", new { erea = "Identity", token, usrid = user.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email!, "Ecomerse -_-_-.. Resend ",
              $"<h1>Confirm Your email clicking <a href='{link}'>Here</a></h1>");

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ForgetPasswoed()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPasswoed(ForgetPasswordVM forgetPasswordVM ,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(forgetPasswordVM);

            var user = await _userManager.FindByNameAsync(forgetPasswordVM.usernameOREmail) ?? await _userManager.FindByEmailAsync(forgetPasswordVM.usernameOREmail);


            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "invalid user Name OR Email ");
                return View(forgetPasswordVM);
            }

           var userOtps =  await _applicationUserOtpRepository.GetAllAsync(E => E.Applicationuserid == user.Id);
            var totalotps = userOtps.Count(e=>(DateTime.UtcNow-e.CreateAt).TotalHours<24);
            if(totalotps>8)
            {
                ModelState.AddModelError(string.Empty, "Too many Atteps");
                return View(forgetPasswordVM);
            }

            var Otp = new Random().Next(1000, 9999).ToString();
            await _applicationUserOtpRepository.AddAsync(new()
            {
                id = Guid.NewGuid().ToString(),
                Applicationuserid = user.Id,
                CreateAt = DateTime.UtcNow,
                Isvalid = true,
                Otp = Otp,
                Validto= DateTime.UtcNow.AddDays(1),

            }, cancellationToken: cancellationToken);

            await _applicationUserOtpRepository.commitASync(cancellationToken);
           
            await _emailSender.SendEmailAsync(user.Email!, "Ecomerse -_-_-.. Resend to password ",
             $"<h1>resent to Otp:{Otp} to resent Your Acount </h1>");

            return RedirectToAction("ValidateOtp", new { userid = user.Id });
        }
        [HttpGet]
        public IActionResult ValidateOtp(string userid)
        {
            return View(new VolidateOtpVM
            {
                Applicationuserid = userid,
            });
        }
        [HttpPost]
        public async Task<IActionResult> ValidateOtp(VolidateOtpVM volidateOtpVM, string userid)
        {
           var result = await _applicationUserOtpRepository.GetoneAsync(e=>e.Applicationuserid == volidateOtpVM.Applicationuserid && e.Otp== volidateOtpVM.Otp && e.Isvalid);
        
          if(result is null)
            {
                ModelState.AddModelError(string.Empty, "invalid Otp");
                return RedirectToAction(nameof(ValidateOtp) ,new {userid = volidateOtpVM.Applicationuserid });
            }

            return RedirectToAction("NewPassword", new { userid = result.Applicationuserid});
        }

        [HttpGet]
        public IActionResult NewPassword(string userid)
        {
            return View(new NewPasswordVM
            {
                Applicationuserid=userid,

            });
        }
        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordVM newPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(newPasswordVM);

            var user = await _userManager.FindByIdAsync(newPasswordVM.Applicationuserid);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "ivalid user");
                return View(newPasswordVM);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result= await _userManager.ResetPasswordAsync(user, token,newPasswordVM.Password);


            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Code);
                }
                return View(newPasswordVM);
            }
            return RedirectToAction("Login");
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

          var user =  await _userManager.FindByNameAsync(loginVM.usernameOREmail) ?? await _userManager.FindByEmailAsync(loginVM.usernameOREmail);


            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "invalid user Name | Email OR password");
                return View(loginVM);
            }

            var p = await _signInManager.PasswordSignInAsync(user, loginVM.password, loginVM.RememberMe, lockoutOnFailure: true);

            if(!p.Succeeded)
            {
                if (p.IsLockedOut)
                    ModelState.AddModelError(string.Empty, "to many attemps, try again often 5 min");
                else if (!user.EmailConfirmed)
                    ModelState.AddModelError(string.Empty, "please confirm Email");
                else
                    ModelState.AddModelError(string.Empty, "invalid user Name | Email OR password");
                return View(loginVM);
            }
            return RedirectToAction("index", "Home", new { area = "Customer" });
        }
    }
}
