namespace HR_SystemProject.Models
{
    public class Department
    {
        public int ID { set; get; }
        public string Name { set; get; }

        public virtual List<Employee> employees {  set; get; }
    }
}
