using ASP.NETSchool.Models;
using ASP.NETSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NETSchool.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;
        private IPasswordValidator<AppUser> _passwordValidator;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser
                {
                    UserName = userViewModel.Name,
                    Email = userViewModel.Email,
                };
                var result = await _userManager.CreateAsync(newUser, userViewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(userViewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            AppUser userToEdit = await _userManager.FindByIdAsync(id);
            if (userToEdit == null)
            {
                return View("NotFound");
            }
            else
            {
                return View(userToEdit);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            AppUser userToEdit = await _userManager.FindByIdAsync(id);
            if (userToEdit == null)
            {
                ModelState.AddModelError("", "User not found"); // nebo return View("NotFound")
                return View();
            }
            else
            {
                IdentityResult validPassword = null;
                if (!string.IsNullOrEmpty(email))
                {
                    userToEdit.Email = email;
                }
                else
                {
                    ModelState.AddModelError("", "E-mail must not be empty");
                }
                if (!string.IsNullOrEmpty(password))
                {
                    validPassword = await _passwordValidator.ValidateAsync(_userManager, userToEdit, password);
                    if (validPassword.Succeeded == true)
                    {
                        userToEdit.PasswordHash = _passwordHasher.HashPassword(userToEdit, password);
                    }
                    else
                    {
                        foreach (var error in validPassword.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Empty password");
                }
                if (validPassword != null && validPassword.Succeeded)
                {
                    var saveResult = await _userManager.UpdateAsync(userToEdit);
                    if (saveResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in saveResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return View(userToEdit);
        }

        public async Task<IActionResult> Delete(string id)
        {
            AppUser userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete == null)
            {
                ModelState.AddModelError("", "User not found");
            }
            else
            {
                var deleteResult = await _userManager.DeleteAsync(userToDelete);
                if (!deleteResult.Succeeded)
                {
                    ModelState.AddModelError("", "Delete unsuccessfull");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
