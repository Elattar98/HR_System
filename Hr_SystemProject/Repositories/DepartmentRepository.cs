using HR_SystemProject.Models;

namespace HR_SystemProject.Repositories
{
    public class DepartmentRepository: IDepartmentRepository
    {
        HrEntity context;
        public DepartmentRepository(HrEntity myDB)
        {
            context = myDB;
        }
        public List<Department> getAll()
        {
            List<Department> departmentList = context.Department.ToList();
            return departmentList;
        }
    }
}
