using HR_SystemProject.Models;
using Microsoft.EntityFrameworkCore;
using HR_SystemProject.ViewModel;

namespace HR_SystemProject.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        //Database..
        HrEntity DB;
        public AttendanceRepository(HrEntity DB)
        {
            this.DB = DB;
        }

        public List<Attendence> GetAll()
        {
            return DB.Attendence.Where(x=>x.IsDeleted!=true).Include(a => a.employee).ToList();
        }

        //Select single attendance data using PK..
        public Attendence GetById(DateOnly date, int Emp_Id)
        {
            return DB.Attendence.FirstOrDefault(a => a.Emp_Id == Emp_Id && a.date == date);
        }

        //Add new attendance record..
        public void AddAttendance(AttendanceViewModel newAttend)
        {
            Attendence newAttendance = new Attendence();
            newAttendance.date = DateOnly.FromDateTime(newAttend.date);
            newAttendance.Emp_Id = newAttend.Emp_Id;
            newAttendance.checkIn = TimeOnly.FromDateTime(newAttend.checkIn);
            newAttendance.checkOut = TimeOnly.FromDateTime(newAttend.checkOut);

            DB.Attendence.Add(newAttendance);
            DB.SaveChanges();
        }

        // used to filter attendances by employee name..
        public List<Attendence> GetByEmpName(string EmpName)
        {
            return DB.Attendence.Where(a => a.employee.name.StartsWith(EmpName)).ToList();
        }

        //used to filter attendances by range of dates..
        public List<Attendence> GetbyPeriodOfDate(DateTime start, DateTime end)
        {
            return DB.Attendence.Where(a => a.date >= DateOnly.FromDateTime(start) && a.date <= DateOnly.FromDateTime(end)).ToList();
        }
        

        //Remove..
        public void DeleteAttendance(DateOnly date, int Emp_Id)
        {
            //DateOnly AttendDate = DateOnly.FromDateTime(date);
            var attend = DB.Attendence.Where(x => x.date == date && x.Emp_Id == Emp_Id).FirstOrDefault();
            if(attend!=null)
            {
                attend.IsDeleted = true;
                DB.Attendence.Update(attend);
            }
           
            DB.SaveChanges();
        }

        public List<Attendence> GetById(int empId)
        {
            List<Attendence> attendances = DB.Attendence.Where(a=>a.Emp_Id==empId).ToList();
            return attendances;
        }
    }
}
