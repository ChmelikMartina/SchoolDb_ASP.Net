using Asp.NETSchool.Models;
using Asp.NETSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NETSchool.Controllers {
    [Authorize]
    public class AccountController : Controller {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [AllowAnonymous] //jinak se mi nikdo neznamy nedokaze poprve prihlasit
        public IActionResult Login(string returnUrl) {
            LoginVM login = new LoginVM();
            login.ReturnUrl = returnUrl;
            return View(login);
        }
        [HttpPost]
        [AllowAnonymous]//pokud by se nepodarilo prihlasit, tak at ma moznost to zkusit znovu
        [ValidateAntiForgeryToken]//bezpectnostni 
        public async Task<IActionResult> Login (LoginVM login) {
            if (ModelState.IsValid) {
                var appUser = await userManager.FindByNameAsync(login.Username);
                if (appUser != null) {
                    var signInResult = await signInManager.PasswordSignInAsync(appUser, login.Password, login.Remember, false);
                    if (signInResult.Succeeded) {
                        return Redirect(login.ReturnUrl ?? "/");//2 otazniky a pak v uvozovkach, kam chci presmerovat, at neni null, tak "/" - presmerovani do korene
                    }
                }
                ModelState.AddModelError("","Login failed: Invalid username or password");
            }
            return View(login);
        }
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");//druhe jmeno je controller name
        }
        public ActionResult AccessDenied() {
            return View();
        }
    }
}
