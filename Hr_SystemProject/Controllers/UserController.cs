using Hr_SystemProject.ViewModel;
using HR_SystemProject.Models;
using HR_SystemProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Authorize("Permissions.UserController.Show")]
        public async Task<IActionResult> Show()
        {
            List<ApplicationUser> users = userManager.Users.ToList();
            
            List<UserViewModel> userRoles = new List<UserViewModel>();
            foreach (var user in users)
            {
                List<string> roles = (List<string>)await userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    userRoles.Add(new UserViewModel() { userId= user.Id, UserName = user.Name, RoleName = roles.First().ToString() });
                }
                else
                {
                    userRoles.Add(new UserViewModel() { userId = user.Id, UserName = user.Name, RoleName = "--"});
                }
                
            }
            return View(userRoles);
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
                        return RedirectToAction("Index", "Home");
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
        [Authorize("Permissions.UserController.Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.userId = user.Id;
            userViewModel.Email = user.Email;
            userViewModel.UserName = user.UserName;
            userViewModel.Name = user.Name;
            userViewModel.Password = user.PasswordHash;
            userViewModel.Roles = roleManager.Roles.ToList();
            if(userManager.GetRolesAsync(user).IsFaulted)
            {
                userViewModel.RoleId = await roleManager.GetRoleIdAsync(roleManager.Roles.
                    FirstOrDefault(r => r.Name == userManager.GetRolesAsync(user).Result.FirstOrDefault()));
            }
                
            
            return View(userViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UserViewModel editedUser)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByIdAsync(id);
                user.Email = editedUser.Email;
                user.UserName = editedUser.UserName;
                user.Name = editedUser.Name;
                editedUser.Password=userManager.PasswordHasher.HashPassword(user, editedUser.Password);
                await userManager.ChangePasswordAsync(user, user.PasswordHash, editedUser.Password);
                
                string? role = userManager.GetRolesAsync(user).Result.FirstOrDefault();
                if (role != null)
                await userManager.RemoveFromRoleAsync(user, role);
                IdentityRole Role = roleManager.Roles.FirstOrDefault(r => r.Id == editedUser.RoleId);
                string newrole = Role.Name;
                IdentityResult Result = await userManager.UpdateAsync(user);
                if (Result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, newrole);
                    TempData["message"] = "Updated Successfully";
                }
                
                return RedirectToAction("Show");
            }
            else
            {
                editedUser.Roles = roleManager.Roles.ToList();
                TempData["message"] = "There's a problem. Please Check What you entered again.";
                return View("Edit", editedUser);
            }
        }
        [Authorize("Permissions.UserController.Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            await userManager.DeleteAsync(user);
            TempData["message"] = "Deleted Successfully";
            return RedirectToAction("Show");
        }
    }
}
