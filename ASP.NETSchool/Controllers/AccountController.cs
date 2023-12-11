using ASP.NETSchool.Models;
using ASP.NETSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETSchool.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]    // umožní login i neprihlasenym uzivatelum
        public IActionResult Login(string returnUrl)
        {
            LoginViewModel login = new LoginViewModel();
            login.ReturnUrl = returnUrl;
            return View(login);
        }
        
        [HttpPost]
        [AllowAnonymous]    // umožní login i neprihlasenym uzivatelum
        [ValidateAntiForgeryToken]  // omezuje urcity zpusob utoku (přes odkaz treba smazat usera)
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByNameAsync(login.Username);
                if (appUser != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(appUser, login.Password, login.Remember, false);
                    if (signInResult.Succeeded)
                    {
                        return Redirect(login.ReturnUrl ?? "/");    // pokud je první část null (login.ReturnUrl), tak se provede druhá část, tady redirect do kořene, pokud je returnUrl null
                    }
                }
                ModelState.AddModelError("", "Login failed: Invalid username or password");
            }
            return View(login);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
