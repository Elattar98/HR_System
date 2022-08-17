using Hr_SystemProject.ViewModel;
using HR_SystemProject.Models;
using HR_SystemProject.Repositories;
using HR_SystemProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hr_SystemProject.Controllers
{
    public class SalaryController : Controller
    {
        IAttendanceRepository AttendanceRepository;
        IEmployeeRepository EmployeeRepository;
        IGeneralSettingsRepository GeneralSettingsRepository;
        public SalaryController(IEmployeeRepository EmpRepo, IAttendanceRepository AttendanceRepo, IGeneralSettingsRepository GeneralSettingsRepo)
        {
            AttendanceRepository = AttendanceRepo;
            EmployeeRepository = EmpRepo;
            GeneralSettingsRepository = GeneralSettingsRepo;
        }
        [Authorize("Permissions.SalaryController.Show")]
        public IActionResult Show()
        {
            List<Employee> Employees = EmployeeRepository.GetAll();
            List<Attendence> attendences = new List<Attendence>();
            List<SalaryViewModel> salarys = new List<SalaryViewModel>();
            int addperhour = GeneralSettingsRepository.GetFirst().Bouns;
            int subperhour = GeneralSettingsRepository.GetFirst().Discount;

            foreach (var emp in Employees)
            {
                
                TimeSpan AddedTime = TimeSpan.Zero;
                TimeSpan SubedTime = TimeSpan.Zero;
                attendences = AttendanceRepository.GetById(emp.ID);
                decimal AllAdds = 0;
                decimal AllSubs = 0;
                int abscence = 0;
                foreach (Attendence attenden in attendences)
                {
                    if(attenden.checkIn > emp.checkIn)
                    {
                        SubedTime+= (attenden.checkIn - emp.checkIn);
                    }
                    if(attenden.checkOut > emp.checkOut)
                    {
                        AddedTime += (attenden.checkOut - emp.checkOut);
                    }                  
                    
                }
                //DaysInMonth Function Get Number of Days In Certain Month.
                for (int day = 1; day <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); day++)
                {
                    DateOnly date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, day);
                    string DayName = date.DayOfWeek.ToString();
                    string vacation1 = Enum.GetName(typeof(WeekDays), 1);
                    string vacation2 = Enum.GetName(typeof(WeekDays), 7);
                    bool attended = attendences.Exists(e => e.date == date);
                    if ( vacation1!= DayName && vacation2!= DayName && !attended)
                    {
                        abscence++;
                    }
                }
                AllAdds = AddedTime.Hours * addperhour;
                AllSubs = SubedTime.Hours * subperhour;
                salarys.Add(new SalaryViewModel() 
                { Name=emp.name,
                  AbscenceNumber=abscence,
                  AttendanceNumber=attendences.Count,
                  Salary=emp.salary,
                  Department = emp.department.Name,
                  AddTime = AddedTime.Hours,
                  SubTime = SubedTime.Hours,
                  AllAdd = AllAdds,
                  AllSub = -AllSubs - abscence * (emp.salary / DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)),
                  OverallSalary = emp.salary + AllAdds + (-AllSubs) - abscence*(emp.salary/ DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
                });
            }
     
            
            return View(salarys);
        }
    }
}
