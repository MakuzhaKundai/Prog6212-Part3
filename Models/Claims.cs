using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System.Models
{
    public class Claim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LecturerId { get; set; }

        [ForeignKey("LecturerId")]
        public virtual ApplicationUser Lecturer { get; set; }

        [Required]
        [Display(Name = "Hours Worked")]
        [Range(0.1, 180, ErrorMessage = "Hours must be between 0.1 and 180")]
        public decimal HoursWorked { get; set; }

        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Additional Notes")]
        [StringLength(500)]
        public string AdditionalNotes { get; set; }

        [Display(Name = "Supporting Document")]
        public string DocumentPath { get; set; }

        [Display(Name = "Original File Name")]
        public string OriginalFileName { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending"; // Pending, ApprovedByCoordinator, ApprovedByManager, Rejected, Approved

        [Display(Name = "Date Submitted")]
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        [Display(Name = "Date Processed")]
        public DateTime? DateProcessed { get; set; }

        [Display(Name = "Processed By")]
        public string ProcessedBy { get; set; }
    }
}