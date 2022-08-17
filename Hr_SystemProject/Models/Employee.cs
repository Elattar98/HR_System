using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HR_SystemProject.Models;

namespace HR_SystemProject.Models
{
    public enum Gender { Male, Female  }

    public class Employee
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string address { get; set; }

        public string phone { get; set; }

        [Column(TypeName = "Date")]
        public DateOnly hiredate { get; set; }

        public string gender { get; set; }

        public string nationality { get; set; }
        public string nationalID { get; set; }


        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public TimeOnly checkIn { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public TimeOnly checkOut { get; set; }

        public Decimal salary { get; set; }

        [Column(TypeName ="Date")]
        public DateOnly birthDate { get; set; }
        
        [ForeignKey("department")]
        public int dept_Id { get; set; }

        public virtual Department department { get; set; }

        public virtual List<Attendence> attendences { get; set; }

    }
}
