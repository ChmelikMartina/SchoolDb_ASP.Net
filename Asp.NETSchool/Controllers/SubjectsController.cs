using Asp.NETSchool.Models;
using Asp.NETSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NETSchool.Controllers {
    public class SubjectsController : Controller {
        private SubjectService service;
        public SubjectsController(SubjectService subjectService) {
            this.service = subjectService;
        }
        public async Task<IActionResult> IndexAsync() {
            var allSubjects = await service.GetAllAsync();
            return View(allSubjects);
        }
        [Authorize(Roles = "Admin, Teacher")] //pro vymezeni pristupu pro skupiny..melo by byt nad kazdou metodou
        public IActionResult Create() {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Subject subject) {
            await service.CreateSubjectAsync(subject);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id) {
            var subjectToEdit = await service.GetById(id);
            if (subjectToEdit == null) {
                return View("NotFound");
            }
            return View(subjectToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, [Bind("Id, Name")] Subject subject) {
            await service.UpdateAsync(subject);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Delete(int id) {
            var subjectToDelete = await service.GetById(id);
            if (subjectToDelete == null) {
                return View("NotFound");
            }
            await service.DeleteAsync(subjectToDelete);
            return RedirectToAction("Index");
        }
    }
}
