using HR_SystemProject.Models;
using HR_SystemProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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
        [Authorize("Permissions.RoleController.Show")]
        public IActionResult Show()
        {
            List<IdentityRole> roles = roleManager.Roles.ToList();

            return View(roles);
        }
        [HttpGet]
        [Authorize("Permissions.RoleController.New")]
        //[AllowAnonymous]
        public IActionResult New()
        {
            RoleViewModel roleView = new RoleViewModel();

            foreach(var con in Permissions.controllerDatas)
            {
                    foreach (var permission in Permissions.permissions.Where(p=>p.Contains(con.ControllerName)).ToList() )
                    {
                    if (!roleView.ControllerNames.Contains(permission.Split(".")[1].Split("Controller")[0]))
                    {
                        roleView.ControllerNames.Add(permission.Split(".")[1].Split("Controller")[0]);
                    }
                    roleView.checkBox.Add(new CheckBox() { DisplayValue = permission, IsChecked = false });
                    }
                
            }
            return View(roleView);
        }
        [HttpPost]
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
                roleViewModel.ControllerNames = new List<string>();

                foreach (var con in Permissions.controllerDatas)
                {
                    foreach (var permission in Permissions.permissions.Where(p => p.Contains(con.ControllerName)).ToList())
                    {
                        if (!roleViewModel.ControllerNames.Contains(permission.Split(".")[1].Split("Controller")[0]))
                        {
                            roleViewModel.ControllerNames.Add(permission.Split(".")[1].Split("Controller")[0]);
                        }
                        roleViewModel.checkBox.Add(new CheckBox() { DisplayValue = permission, IsChecked = false });
                    }

                }
                return View(roleViewModel);
            }
            
            return RedirectToAction("New");
        }
        [Authorize("Permissions.RoleController.Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            RoleViewModel roleViewModel = new RoleViewModel();
            roleViewModel.RoleId = role.Id;
            roleViewModel.RoleName = role.Name;
            var roleClaims = roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            var allPermissions = Permissions.permissions;

            foreach (var permission in allPermissions)
            {
                if (!roleViewModel.ControllerNames.Contains(permission.Split(".")[1].Split("Controller")[0]))
                {
                    roleViewModel.ControllerNames.Add(permission.Split(".")[1].Split("Controller")[0]);
                }
                if (roleClaims.Any(c => c == permission))
                {
                    roleViewModel.checkBox.Add(new CheckBox() { DisplayValue = permission, IsChecked = true });
                }
                else
                {
                    roleViewModel.checkBox.Add(new CheckBox() { DisplayValue = permission, IsChecked = false });
                }
                
            }
            return View(roleViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, RoleViewModel editedRole)
         {
            if(editedRole.checkBox==null)
            {
                IdentityRole updatedRole = await roleManager.FindByIdAsync(id);
                updatedRole.Name = editedRole.RoleName;
                return RedirectToAction("Show");
            }
            if (ModelState.IsValid)
            {
                IdentityRole updatedRole = await roleManager.FindByIdAsync(id);
                updatedRole.Name = editedRole.RoleName;
                foreach (var claim in roleManager.GetClaimsAsync(updatedRole).Result)
                {
                    await roleManager.RemoveClaimAsync(updatedRole, claim);
                }
                var selectedClaims = editedRole.checkBox.Where(c => c.IsChecked).ToList();
                foreach (var claim in selectedClaims)
                {
                    await roleManager.AddClaimAsync(updatedRole, new Claim("Permission", claim.DisplayValue));
                }
                
                return RedirectToAction("Show");
            }
            else
            {
                IdentityRole role = await roleManager.FindByIdAsync(id);
                var roleClaims = roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
                var allPermissions = Permissions.permissions;

                foreach (var permission in allPermissions)
                {
                    if (!editedRole.ControllerNames.Contains(permission.Split(".")[1].Split("Controller")[0]))
                    {
                        editedRole.ControllerNames.Add(permission.Split(".")[1].Split("Controller")[0]);
                    }
                    if (roleClaims.Any(c => c == permission))
                    {
                        editedRole.checkBox.Add(new CheckBox() { DisplayValue = permission, IsChecked = true });
                    }
                    else
                    {
                        editedRole.checkBox.Add(new CheckBox() { DisplayValue = permission, IsChecked = false });
                    }

                }
                TempData["message"] = "There's a problem. Please Check What you entered again.";
                return View("Edit", editedRole);
            }
        }
        [Authorize("Permissions.RoleController.Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            await roleManager.DeleteAsync(role);
            TempData["message"] = "Deleted Successfully";
            return RedirectToAction("Show");
        }
    }
}
