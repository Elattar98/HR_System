using System.ComponentModel.DataAnnotations;

namespace HR_SystemProject.ViewModel
{
    public enum WeekDays
    {

        Saturday = 1,
        Sunday = 2,
        Monday = 3,
        Tuesday = 4,
        Wednesday = 5,
        Thursday = 6,
        Friday = 7
    }
    public class GeneralSettingsViewModel
    {
        [Required(ErrorMessage = "من فضلك قم بادخال هذا الحقل ")]
        [Display(Name = "الاضافة")]
        public int Bouns { get; set; }
        [Required(ErrorMessage = "من فضلك قم بادخال هذا الحقل ")]
        [Display(Name = "الخصم")]
        public int Discount { get; set; }
        [Required(ErrorMessage = "من فضلك قم بادخال هذا الحقل ")]
        [Display(Name = "يوم الاجازة الرسمي 1")]
        public WeekDays vacation1 { get; set; }
        [Required(ErrorMessage = "من فضلك قم بادخال هذا الحقل ")]
        [Display(Name = "يوم الاجازة الرسمي 2")]
        public WeekDays vacation2 { get; set; }
    }
}
