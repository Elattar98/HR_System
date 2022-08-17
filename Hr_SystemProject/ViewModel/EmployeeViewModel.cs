using HR_SystemProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HR_SystemProject.ViewModel
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"\d{14}", ErrorMessage = "SSN Must be 14 Numbers.")]
        public string SSN { get; set; }
        [Required]
        [StringLength(40, MinimumLength=2)]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [RegularExpression(@"(010|011|012|015)\d{8}", ErrorMessage = "Phone Must be 11 Numbers Begins with (010,011,012,015)")]
        public string MobileNumber { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [BirthDate]
        [Remote(action: "VerifyDate", controller: "Employee", AdditionalFields = nameof(ContractDate))]
        public DateTime BirthDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [ContractDate]
        [Remote(action: "VerifyDate", controller: "Employee", AdditionalFields = nameof(BirthDate))]
        public DateTime ContractDate { get; set; }
        [Required]
        [DisplayFormatAttribute(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime AttendanceTime { get; set; }
        [Required]
        [DisplayFormatAttribute(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DepartureTime { get; set; }
        [Required]
        //[DisplayFormatAttribute(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public Decimal Salary { get; set; }
        [Display(Name = "Departments")]
        public int DepartmentId { get; set; }
        public List<Department>? Departments { get; set; }
    }
}
