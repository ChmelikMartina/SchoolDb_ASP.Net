using Asp.NETSchool.Services;
using Asp.NETSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asp.NETSchool.Controllers {
    [Authorize(Roles = "Admin, Teacher")]
    public class GradesController : Controller {
        private GradeService service;
        public GradesController(GradeService service)
        {
            this.service = service;
        }
        public async Task <IActionResult> Index() {
            var allGrades = await service.GetAllGrades();
            return View(allGrades);
        }
        [HttpGet]
        public async Task <IActionResult> Create() {
            var gradesDropdownData = await service.GetDropdownValuesAsync();
            ViewBag.Students=new SelectList (gradesDropdownData.Students, "Id", "LastName");
            ViewBag.Subjects=new SelectList(gradesDropdownData.Subjects, "Id", "Name"); 
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> CreateAsync(GradeVM gradeModel) {
            await service.CreateAsync(gradeModel);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var gradesDropdownData = await service.GetDropdownValuesAsync();
            ViewBag.Students = new SelectList(gradesDropdownData.Students, "Id", "LastName");
            ViewBag.Subjects = new SelectList(gradesDropdownData.Subjects, "Id", "Name");
            var gradeToEdit = service.GetById(id);
            return View(gradeToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id,GradeVM updatedGrade) {
            await service.UpdateAsync(updatedGrade);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteAsync(int id) {
            await service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
