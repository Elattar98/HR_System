using System.ComponentModel.DataAnnotations; 

namespace HR_SystemProject.Models.CustomValidations
{
    public class ValidHireDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime StartDate = DateTime.Parse("06/06/2005");
            DateTime HireDate = Convert.ToDateTime(value);

            if (HireDate > StartDate)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Hiring must be after 6/6/2005");
        }
    }
}
