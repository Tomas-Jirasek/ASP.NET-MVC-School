using ASP.NETSchool.Services;
using ASP.NETSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NETSchool.Controllers
{
    [Authorize(Roles ="Teacher, Admin")]
    public class GradesController : Controller
    {
        private GradeService service;

        public GradesController(GradeService gradeService)
        {
            this.service = gradeService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var allGrades = await service.GetAllGradesAsync();
            return View(allGrades);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var gradesDropdownsData = await service.GetDropdownValuesAsync();
            // v parametrech je skupina dat, value selectu je Id, zobrazuje se LastName
            ViewBag.Students = new SelectList(gradesDropdownsData.Students, "Id", "LastName");
            ViewBag.Subjects = new SelectList(gradesDropdownsData.Subjects, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(GradeViewModel gradeViewModel) // vraci se presmerovani, proto zase IActionResult
        {
            await service.CreateAsync(gradeViewModel);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var gradesDropdownsData = await service.GetDropdownValuesAsync();
            ViewBag.Students = new SelectList(gradesDropdownsData.Students, "Id", "LastName");
            ViewBag.Subjects = new SelectList(gradesDropdownsData.Subjects, "Id", "Name");
            var gradeToEdit = service.GetById(id);
            if (gradeToEdit == null) 
            {
                return View("NotFound");
            }
            return View(gradeToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, GradeViewModel updatedGrade)
        {
            await service.UpdateAsync(updatedGrade);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
