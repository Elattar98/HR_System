using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HR_SystemProject.ViewModel
{
    public class UserViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name="Roles")]
        public string RoleId { get; set; }
        public List<IdentityRole>? Roles { get; set; }
        //[Required]
        //public string? RoleId { get; set; }
    }
}
