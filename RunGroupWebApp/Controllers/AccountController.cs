using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {

            if(!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.Email);

            if (user != null)
            {
                // user is found passwordCheck
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if(passwordCheck)
                {
                    // pass correct signin
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded) { 
                     return RedirectToAction("Index", "Home");
                    }
                }
                //pass incorrect
                TempData["Error"] = "Wrong credenctials. Please, try again";
                return View(loginVM);
            }

            // user not found
            TempData["Error"] = "Wrong credenctials. Please, try again";
            return View(loginVM);

        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if(!ModelState.IsValid) return View(registerVM);
            var user = await _userManager.FindByEmailAsync(registerVM.Email);
            if(user == null) // null because is an object - an integer is 0
            {
                TempData["Error"] = "This email is already in use";
                return View(registerVM);
            }

            var newUser = new AppUser()
            {
                Email = registerVM.Email,
                UserName = registerVM.Email,
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
