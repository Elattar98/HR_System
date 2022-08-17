using HR_SystemProject.Models;
using HR_SystemProject.ViewModel;

namespace HR_SystemProject.Repositories
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetAll();
        public Employee GetByID(int id);
        public void Insert(EmployeeViewModel emp);
        public void Update(int id, EmployeeViewModel editedemp);
        public void Delete(int id);
    }
}
