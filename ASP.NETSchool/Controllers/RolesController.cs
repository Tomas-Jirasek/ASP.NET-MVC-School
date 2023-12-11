using ASP.NETSchool.Models;
using ASP.NETSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETSchool.Controllers
{
    [Authorize(Roles = "Admin")] 
    public class RolesController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult createResult = await _roleManager.CreateAsync(new IdentityRole { Name = name });
                if (createResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(name);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id) // z tlacitka delete v indexu jde asp-route-id, coz je string
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);
            if (roleToDelete != null)
            {
                var deleteResult = await _roleManager.DeleteAsync(roleToDelete);
                if (!deleteResult.Succeeded)
                {
                    foreach (var error in deleteResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            ModelState.AddModelError("", "Role not found");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            if (role != null)
            {
                foreach (var user in _userManager.Users)
                {
                    var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                    list.Add(user);
                }
                return View(new RoleEditViewModel
                {
                    Members = members,
                    NonMembers = nonMembers,
                    Role = role
                });
            }
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleModificationViewModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit", "Roles", model.RoleId);
            }
        }
    }
}
