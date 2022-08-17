using HR_SystemProject.Models;
using HR_SystemProject.Repositories;
using HR_SystemProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HR_SystemProject.Controllers
{
    public class EmployeeController : Controller
    {
        IEmployeeRepository EmployeeRepo;
        IDepartmentRepository DepartmentRepo;
        public EmployeeController(IEmployeeRepository EmpRepo, IDepartmentRepository DeptRepo)
        {
            EmployeeRepo = EmpRepo;
            DepartmentRepo = DeptRepo;
        }
        [Authorize("Permissions.EmployeeController.ShowAll")]
        public IActionResult ShowAll()
        {
            List<Employee> employees = EmployeeRepo.GetAll();
            return View(employees);
        }
        [Authorize("Permissions.EmployeeController.New")]
        public IActionResult New()
        {
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            employeeViewModel.Departments = DepartmentRepo.getAll();
            return View(employeeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(EmployeeViewModel emp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmployeeRepo.Insert(emp);
                    TempData["message"] = "Added Successfully";
                    return RedirectToAction("ShowAll");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(String.Empty, ex.Message.ToString());
                    return View("New", emp);

                }
                
            }
            else
            {
                emp.Departments = DepartmentRepo.getAll();
                TempData["message"] = "There's a problem. Please Check What you entered again.";
                return View("New", emp);
            }

        }
        [Authorize("Permissions.EmployeeController.Edit")]
        public IActionResult Edit(int id)
        { 
            Employee employee = EmployeeRepo.GetByID(id);
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            employeeViewModel.Address = employee.address;
            employeeViewModel.AttendanceTime += employee.checkIn.ToTimeSpan();
            employeeViewModel.BirthDate = employee.birthDate.ToDateTime(TimeOnly.MinValue);
            employeeViewModel.ContractDate = employee.hiredate.ToDateTime(TimeOnly.MinValue);
            employeeViewModel.DepartmentId = employee.dept_Id;
            employeeViewModel.DepartureTime += employee.checkOut.ToTimeSpan();
            employeeViewModel.Id = employee.ID;
            employeeViewModel.Gender = (Gender)Enum.Parse(typeof(Gender), employee.gender);
            employeeViewModel.MobileNumber = employee.phone;
            employeeViewModel.Name = employee.name;
            employeeViewModel.Nationality = employee.nationality;
            employeeViewModel.Salary = employee.salary;
            employeeViewModel.SSN = employee.nationalID;
            employeeViewModel.Departments = DepartmentRepo.getAll();
            return View(employeeViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, EmployeeViewModel editedEmployee)
        {

            if (ModelState.IsValid)
            {
                EmployeeRepo.Update(id, editedEmployee);
                TempData["message"] = "Updated Successfully";
                return RedirectToAction("ShowAll");
            }
            else
            {
                editedEmployee.Departments = DepartmentRepo.getAll();
                TempData["message"] = "There's a problem. Please Check What you entered again.";
                return View("Edit", editedEmployee);
            }
        }
        [Authorize("Permissions.EmployeeController.Delete")]
        public IActionResult Delete(int id)
        {
            EmployeeRepo.Delete(id);
            TempData["message"] = "Deleted Successfully";
            return RedirectToAction("ShowAll");
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyDate(DateTime BirthDate, DateTime ContractDate)
        {
            if (BirthDate>=ContractDate)
            {
                return Json("BirthDate must be less than ContractDate.");
            }

            return Json(true);
        }
    }
}
