using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Hourly Rate")]
        [Range(0, double.MaxValue, ErrorMessage = "Hourly rate must be positive")]
        public decimal HourlyRate { get; set; }

        [Required]
        public string Role { get; set; } // "Lecturer", "ProgrammeCoordinator", "AcademicManager", "HR"

        // Navigation properties
        public virtual ICollection<Claim> Claims { get; set; }
    }
}