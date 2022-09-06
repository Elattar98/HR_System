using HR_SystemProject.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HR_SystemProject.ViewModel
{
    public class AttendanceViewModel
    {
        [Required(ErrorMessage ="Date must be selected")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }

        [Required(ErrorMessage ="Please select employee")]
        [Display(Name ="Employee")]
        public int Emp_Id { get; set; }

        [Required(ErrorMessage ="Select Attendance time")]
        [DisplayFormatAttribute(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime checkIn { get; set; }

        [Required(ErrorMessage ="Select Departure time")]
        [DisplayFormatAttribute(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime checkOut { get; set; }

        public List<Employee>? employees { get; set; }

    }
}
