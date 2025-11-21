using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }

        [Required]
        [Display(Name = "Lecturer ID")]
        public int LecturerId { get; set; }

        [Required]
        [Display(Name = "Month")]
        public string Month { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Hours Worked")]
        [Range(0, double.MaxValue)]
        public decimal HoursWorked { get; set; }

        [Required]
        [Display(Name = "Hourly Rate")]
        [Range(0, double.MaxValue)]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Documents")]
        public string? Documents { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Date Submitted")]
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        [Display(Name = "Submission Date")]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        [Display(Name = "Date Approved")]
        public DateTime? DateApproved { get; set; }

        [Display(Name = "Approval Date")]
        public DateTime? ApprovalDate { get; set; }

        [Display(Name = "Approved By")]
        public string? ApprovedBy { get; set; }

        [Display(Name = "Rejection Reason")]
        public string? RejectionReason { get; set; }

        // Computed properties
        [Display(Name = "Total Amount")]
        public decimal TotalAmount => HoursWorked * HourlyRate;

        [Display(Name = "Claim Amount")]
        public decimal ClaimAmount => HoursWorked * HourlyRate;

        [Display(Name = "Lecturer Name")]
        public string LecturerName => Lecturer?.FullName ?? "Unknown";

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Lecturer? Lecturer { get; set; }
    }
}