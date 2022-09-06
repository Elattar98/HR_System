using Microsoft.AspNetCore.Mvc;
using HR_SystemProject.Repositories;
using HR_SystemProject.Models;
using HR_SystemProject.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace HR_SystemProject.Controllers
{
    public class AttendanceController : Controller
    {
        IAttendanceRepository AttendanceRepository;
        IEmployeeRepository EmployeeRepository;
        public AttendanceController(IAttendanceRepository AttendanceRepo, IEmployeeRepository EmployeeRepo)
        {
            AttendanceRepository = AttendanceRepo;
            EmployeeRepository = EmployeeRepo;
        }
        [Authorize("Permissions.AttendanceController.New")]
        public IActionResult New()
        {
            AttendanceViewModel model = new AttendanceViewModel();
            model.employees = EmployeeRepository.GetAll();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(AttendanceViewModel newattendance)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AttendanceRepository.AddAttendance(newattendance);
                    TempData["message"] = "Added Successfully";
                    return RedirectToAction("New");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(String.Empty, ex.Message.ToString());
                    return View("New", newattendance);

                }

            }
            else
            {
                newattendance.employees = EmployeeRepository.GetAll();
                TempData["message"] = "There's a problem. Please Check What you entered again.";
                return View("New", newattendance);
            }
        }
        [Authorize("Permissions.AttendanceController.Show")]
        public IActionResult Show()
        {
            List<Attendence> attendences = AttendanceRepository.GetAll();
            return View(attendences);
        }
        [Authorize("Permissions.AttendanceController.Delete")]
        public IActionResult Delete(DateTime date, int empId)
        {
            AttendanceRepository.DeleteAttendance(DateOnly.FromDateTime(date), empId);
            TempData["message"] = "Deleted Successfully";
            return RedirectToAction("Show");
        }
        [Authorize("Permissions.AttendanceController.Edit")]
        public IActionResult Edit(DateTime date, int empId)
        {
            Attendence attendence = AttendanceRepository.GetById(DateOnly.FromDateTime(date), empId);
            AttendanceViewModel model = new AttendanceViewModel();
            model.date = attendence.date.ToDateTime(TimeOnly.MinValue);
            model.checkOut = DateTime.MinValue.Add(attendence.checkOut.ToTimeSpan());
            model.checkIn = DateTime.MinValue.Add(attendence.checkIn.ToTimeSpan());
            model.Emp_Id = empId;
            model.employees = EmployeeRepository.GetAll();
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(DateTime date, int empId, AttendanceViewModel attendanceViewModel)
        {
            if(ModelState.IsValid)
            {
                AttendanceRepository.UpdateAttendance(DateOnly.FromDateTime(date), empId, attendanceViewModel);
                TempData["message"] = "Updated Successfully";
                return RedirectToAction("Show");
            }
            else
            {
                attendanceViewModel.employees = EmployeeRepository.GetAll();
                TempData["message"] = "There's a problem. Please Check What you entered again.";
                return View("Edit", attendanceViewModel);
            }
        }
        public IActionResult Search(string searchText, DateTime startDate, DateTime endDate)
        {
            List<Attendence> attendences = AttendanceRepository.GetAll();
            attendences = attendences.
                Where(a => a.date.ToDateTime(TimeOnly.MinValue) <= endDate && a.date.ToDateTime(TimeOnly.MinValue) >= startDate).ToList();
            if (searchText!=null)
            {
                List<Attendence> result = attendences.
                    Where(a => a.employee.name.ToLower().Contains(searchText.ToLower())||a.employee.department.Name.ToLower().Contains(searchText.ToLower())).ToList();
                return PartialView("_AttendanceTableViewPartial", result);
            }
            return PartialView("_AttendanceTableViewPartial", attendences);
            
        }
    }
}
