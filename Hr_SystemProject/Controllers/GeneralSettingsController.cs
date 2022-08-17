using HR_SystemProject.Repositories;
using HR_SystemProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HR_SystemProject.Controllers
{
    public class GeneralSettingsController : Controller
    {
        IGeneralSettingsRepository GeneralSettingsRepo;
        public GeneralSettingsController(IGeneralSettingsRepository GeneralSetRepo)
        {
            GeneralSettingsRepo = GeneralSetRepo;
        }
        [HttpGet]
        [Authorize("Permissions.GeneralSettingsController.Update")]
        public IActionResult Update()
        {
            GeneralSettingsViewModel generalSettingsViewModel = new GeneralSettingsViewModel();
            return View(generalSettingsViewModel);
        }

        [HttpPost]
        public IActionResult Save(GeneralSettingsViewModel generalSettingsView)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GeneralSettingsRepo.Update(generalSettingsView);
                    TempData["message"] = "Added Successfully";
                    TempData["AddClass"] = "success";
                    return View("Update");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message.ToString());
                    return View("Update");

                }

            }
            else
            {
                TempData["message"] = "There's a problem. Please Check What you entered again.";
                TempData["AddClass"] = "danger";
                return View("Update");
            }
        }
    }
}
