using HR_SystemProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace HR_SystemProject.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        [HttpGet]
        [Authorize("Permissions.RoleController.New")]
        //[AllowAnonymous]
        public IActionResult New()
        {
            RoleViewModel roleView = new RoleViewModel();
            roleView.checkBox = new List<CheckBox>();
            roleView.ControllerNames = new List<string>();
            Permissions permissions = new Permissions();

            foreach(var con in permissions.controllerDatas)
            {
                if (!roleView.ControllerNames.Contains(con.ControllerName))
                {
                    roleView.ControllerNames.Add(con.ControllerName);
                }
                    foreach (var act in con.Actions )
                    {
                        roleView.checkBox.Add(new CheckBox() { DisplayValue = act.DisplayValue, IsChecked = false });
                    }
                
            }
            return View(roleView);
        }
        [HttpPost]
        [Authorize("Permissions.RoleController.New")]
        //[AllowAnonymous]
        public async Task<IActionResult> New(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid == true)
            {
                IdentityRole newRole = new IdentityRole();
                newRole.Name = roleViewModel.RoleName;
                IdentityResult Result = await roleManager.CreateAsync(newRole);
                if (Result.Succeeded)
                {
                        foreach (var cb in roleViewModel.checkBox)
                        {
                            if (cb.IsChecked!=false)
                            {
                                await roleManager.AddClaimAsync(newRole, new Claim("Permission", cb.DisplayValue));
                            }

                        }
                    TempData["message"] = "Added New Role Successfully";
                }
                else
                {
                    foreach (var erroritem in Result.Errors)
                    {
                        ModelState.AddModelError(erroritem.Code, erroritem.Description);
                    }
                }
            }
            else
            {
                roleViewModel.checkBox = new List<CheckBox>();
                roleViewModel.ControllerNames = new List<string>();
                Permissions permissions = new Permissions();

                foreach (var con in permissions.controllerDatas)
                {
                    if (!roleViewModel.ControllerNames.Contains(con.ControllerName))
                    {
                        roleViewModel.ControllerNames.Add(con.ControllerName);
                    }
                    foreach (var act in con.Actions)
                    {
                        roleViewModel.checkBox.Add(new CheckBox() { DisplayValue = act.DisplayValue, IsChecked = false });
                    }

                }
                return View(roleViewModel);
            }
            
            return RedirectToAction("New");
        }
    }
}
