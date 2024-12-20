﻿namespace Hr_SystemProject.ViewModel
{
    public class SalaryViewModel
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public int AttendanceNumber { get; set; }
        public int AbscenceNumber { get; set; }
        public double AddTime { get; set; }
        public double SubTime { get; set; }
        public decimal AllAdd { get; set; }
        public decimal AllSub { get; set; }
        public decimal OverallSalary { get; set; }
    }
}
