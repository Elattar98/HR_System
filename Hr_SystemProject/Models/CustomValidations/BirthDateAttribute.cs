using System.ComponentModel.DataAnnotations;

namespace HR_SystemProject.Models
{
    public class BirthDateAttribute : ValidationAttribute
    {
        public string GetErrorMessage() =>
             "Employee Age Must be Over 20. ";

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value!;
            DateTime now = DateTime.Now;
            TimeSpan span = now.AddYears(20) - now;

            if (DateTime.Now.Subtract(date) < span)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}
