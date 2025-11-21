using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class User
    {
        public int UserId { get; set; }

        public int Id => UserId;

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "ID Number")]
        public string? IdNumber { get; set; }

        [Display(Name = "Bank Name")]
        public string? BankName { get; set; }

        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }

        [Display(Name = "Branch Code")]
        public string? BranchCode { get; set; }

        [Display(Name = "Hourly Rate")]
        [Range(0, double.MaxValue)]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        // Computed properties
        [Display(Name = "Name")]
        public string Name => $"{FirstName} {LastName}";

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation property
        public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();
    }
}