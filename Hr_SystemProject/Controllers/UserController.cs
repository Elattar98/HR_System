using HR_SystemProject.Models;
using HR_SystemProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HR_SystemProject.Controllers
{
    
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [HttpGet]
        [Authorize("Permissions.UserController.Add")]
        //[AllowAnonymous]
        public IActionResult Add()
        {
            UserViewModel userView = new UserViewModel();
            userView.Roles = roleManager.Roles.ToList();
            return View(userView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Permissions.UserController.Add")]
        //[AllowAnonymous]
        public async Task<IActionResult> Add(UserViewModel newuser)
        {
            newuser.Roles = roleManager.Roles.ToList();
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.Name = newuser.Name;
                applicationUser.PasswordHash = newuser.Password;
                applicationUser.UserName = newuser.UserName;
                applicationUser.Email = newuser.Email;

                IdentityRole Role = roleManager.Roles.FirstOrDefault(r=>r.Id==newuser.RoleId);
                string RoleName = Role.Name;
                IdentityResult result = await userManager.CreateAsync(applicationUser, newuser.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicationUser, RoleName);

                    return RedirectToAction("Add");
                }
                else
                {
                    foreach (var erroritem in result.Errors)
                    {
                        ModelState.AddModelError("Password", erroritem.Description);
                    }
                }
            
            }
            
            return View(newuser);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser userModel = await userManager.FindByNameAsync(loginUserViewModel.Username);
                if(userModel!=null)
                {
                    bool found = await userManager.CheckPasswordAsync(userModel, loginUserViewModel.Password);
                    if(found)
                    {
                        await signInManager.SignInAsync(userModel, loginUserViewModel.RememberMe);
                        return RedirectToAction("ShowAll", "Employee");
                    }
                }
                ModelState.AddModelError("", "Username or Password is wrong");
            }
            return View(loginUserViewModel);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            if (signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
                
            }

            return RedirectToAction("Login");
        }
    }
}
