using Asp.NETSchool.Models;
using Asp.NETSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace Asp.NETSchool.Controllers {
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller {
        //identity balicek uz v sobe servisku ma, Manager, tak ji nemusim psat
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;
        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator) {
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.passwordValidator = passwordValidator;
        }
        public IActionResult Index() {
            return View(userManager.Users);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserVM userModel) {
            if (ModelState.IsValid) {
                AppUser newUser = new AppUser {
                    UserName = userModel.Name,
                    Email = userModel.Email
                };
                var result = await userManager.CreateAsync(newUser, userModel.Password);//password vyplnujeme az skrz metodu pridanim jako 2.parametr
                if (result.Succeeded) {
                    return RedirectToAction("Index");//kdyz projde, vratim se do Indexu
                }
                else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(userModel);//kdyz neprojde, vrati me do vyplnovani uzivatele i s predvyplnenymi udaji
        }
        public async Task<IActionResult> Edit(string id) {
            AppUser userToEdit = await userManager.FindByIdAsync(id);
            if (userToEdit == null) {
                return View("NotFound");
            }
            else {
                return View(userToEdit);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password) {
            AppUser userToEdit = await userManager.FindByIdAsync(id);
            if (userToEdit == null) {
                ModelState.AddModelError("", "User not found");//jen pro sichr, nemelo by se stat..jen v pripade, ze by mi nekdo primo pod rukama vymazal uzivatele
                return View();
            }
            //else if (password is null || email is null) {return View(userToEdit);}  jednodussi cesta
            else {
                IdentityResult validPassword = null;
                if (!string.IsNullOrEmpty(email)) {
                    userToEdit.Email = email;
                }
                else {
                    ModelState.AddModelError("", "Email must not be empty");
                }
                if (!string.IsNullOrEmpty(password)) {
                    validPassword = await passwordValidator.ValidateAsync(userManager, userToEdit, password);
                    if (validPassword.Succeeded) {
                        userToEdit.PasswordHash = passwordHasher.HashPassword(userToEdit, password);
                    }
                    else {
                        foreach (var error in validPassword.Errors) {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else {
                    ModelState.AddModelError("", "Empty password!");
                }
                if (validPassword!=null && validPassword.Succeeded) {
                    var saveResult = await userManager.UpdateAsync(userToEdit);
                    if (saveResult.Succeeded) {
                        return RedirectToAction("Index");
                    }
                    else {
                        foreach (var error in saveResult.Errors) {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return View(userToEdit);
        }
        public async Task<IActionResult> Delete(string id) {
            AppUser userToDelete = await userManager.FindByIdAsync(id);
            if (userToDelete == null) {
                ModelState.AddModelError("", "User not found");
            }
            else {
                var deleteResult = await userManager.DeleteAsync(userToDelete);
                if (!deleteResult.Succeeded) {
                    ModelState.AddModelError("", "Delete unsuccessfull");

                }
            }
            return RedirectToAction("Index");
        }
    }
}
