using HR_SystemProject.Models;
using HR_SystemProject.ViewModel;

namespace HR_SystemProject.Repositories
{
    public interface IAttendanceRepository
    {
        List<Attendence> GetAll();
        Attendence GetById(DateOnly date, int Emp_Id);
        void AddAttendance(AttendanceViewModel newAttend);
        void DeleteAttendance(DateOnly date, int Emp_Id);

        List<Attendence> GetByEmpName(string name);
        List<Attendence> GetById(int empId);
        List<Attendence> GetbyPeriodOfDate(DateTime start, DateTime end);


    }
}
