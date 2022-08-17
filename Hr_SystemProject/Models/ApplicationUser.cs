using Microsoft.AspNetCore.Identity;
using HR_SystemProject.Models;

namespace HR_SystemProject.Models
{
    public class ApplicationUser : IdentityUser /* hr */
    {
        public string Name { get; set; }
        /*
         * name
         * *** In IdentityUser ***
         * email
         * username
         * password
         */
    }
}
