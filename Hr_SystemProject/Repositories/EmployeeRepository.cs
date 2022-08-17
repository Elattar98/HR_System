using HR_SystemProject.Models;
using HR_SystemProject.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace HR_SystemProject.Repositories
{
    public class EmployeeRepository: IEmployeeRepository
    {
        HrEntity context;

        public EmployeeRepository(HrEntity myDB)
        {
            context = myDB;
        }

        public void Delete(int id)
        {
            Employee deletedemployee = GetByID(id);
            context.Employee.Remove(deletedemployee);
            context.SaveChanges();
        }

        public List<Employee> GetAll()
        {
            List<Employee> employees = context.Employee.Include(d=>d.department).ToList();
            return employees;
        }

        public Employee GetByID(int id)
        {
            Employee employee = context.Employee.SingleOrDefault(e => e.ID == id);
            return employee;
        }

        public void Insert(EmployeeViewModel emp)
        {
            Employee newemp = new Employee();
            newemp.nationalID = emp.SSN;
            newemp.name = emp.Name;
            newemp.phone = emp.MobileNumber;
            newemp.nationality = emp.Nationality;
            newemp.gender = Enum.GetName(typeof(Gender), emp.Gender);
            newemp.address = emp.Address;
            newemp.checkOut = TimeOnly.FromDateTime(emp.DepartureTime);
            newemp.dept_Id = emp.DepartmentId;
            newemp.hiredate = DateOnly.FromDateTime(emp.ContractDate);
            newemp.birthDate = DateOnly.FromDateTime(emp.BirthDate);
            newemp.checkIn = TimeOnly.FromDateTime(emp.AttendanceTime);
            newemp.salary = emp.Salary;
            context.Employee.Add(newemp);
            context.SaveChanges();
        }

        public void Update(int id, EmployeeViewModel editedemp)
        {
            Employee updatedemployee = GetByID(id);
            updatedemployee.nationalID = editedemp.SSN;
            updatedemployee.name = editedemp.Name;
            updatedemployee.phone = editedemp.MobileNumber;
            updatedemployee.nationality = editedemp.Nationality;
            updatedemployee.gender = Enum.GetName(typeof(Gender), editedemp.Gender);
            updatedemployee.address = editedemp.Address;
            updatedemployee.checkOut = TimeOnly.FromDateTime(editedemp.DepartureTime);
            updatedemployee.dept_Id = editedemp.DepartmentId;
            updatedemployee.hiredate = DateOnly.FromDateTime(editedemp.ContractDate);
            updatedemployee.birthDate = DateOnly.FromDateTime(editedemp.BirthDate);
            updatedemployee.checkIn = TimeOnly.FromDateTime(editedemp.AttendanceTime);
            updatedemployee.salary = editedemp.Salary;
            context.SaveChanges();
        }
    }
}
