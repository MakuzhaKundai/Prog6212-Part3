using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class Lecturer
    {
        public int LecturerId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Department")]
        public string Department { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Contract Type")]
        public string ContractType { get; set; } = string.Empty;

        [Display(Name = "Hourly Rate")]
        [Range(0, double.MaxValue)]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Monthly Salary")]
        [Range(0, double.MaxValue)]
        public decimal MonthlySalary { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        // Computed property
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation property
        public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();
    }
}