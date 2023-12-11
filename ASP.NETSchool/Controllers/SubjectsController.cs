using ASP.NETSchool.Models;
using ASP.NETSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETSchool.Controllers
{
    public class SubjectsController : Controller
    {
        public SubjectService service;

        public SubjectsController(SubjectService subjectService)
        {
            this.service = subjectService;
        }
        public async Task<IActionResult> IndexAsync()   // async za indexem .net core ignoruje
        {
            var allSubjects = await service.GetAllAsync();
            return View(allSubjects);
        }
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Subject subject)
        {
            await service.CreateSubjectAsync(subject);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Edit(int id)
        {
            var subjectToEdit = await service.GetById(id);
            if (subjectToEdit == null)
            {
                return View("NotFound");
            }
            return View(subjectToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, [Bind("Id, Name")] Subject subject)
        {
            await service.UpdateAsync(subject);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var subjectToDelete = await service.GetById(id);
            if (subjectToDelete == null)
            {
                return View("NotFound");
            }
            await service.DeleteAsync(subjectToDelete);
            return RedirectToAction("Index");
        }
    }
}
