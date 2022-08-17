using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HR_SystemProject.Models
{
    public class Attendence
    {
        // composite PKey ..1
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly date { get; set; }

        // composite PKey ..2
        [ForeignKey("employee")]
        public int Emp_Id { get; set; }

        public bool?  IsDeleted { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public TimeOnly checkIn { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public TimeOnly checkOut { get; set; }

        public virtual Employee? employee { set; get; }



    }
}
