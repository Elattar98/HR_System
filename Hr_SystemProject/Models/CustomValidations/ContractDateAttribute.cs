using System.ComponentModel.DataAnnotations;

namespace HR_SystemProject.Models
{
    public class ContractDateAttribute : ValidationAttribute
    {
        public string GetErrorMessage() =>
             "Please Enter Correct Contraction Date. ";

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value!;
            if (date.Year - 2008 < 0)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}