using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Services
{
    public class PdfReportService : IReportService
    {
        private readonly IClaimService _claimService;
        private readonly ILecturerService _lecturerService;

        public PdfReportService(IClaimService claimService, ILecturerService lecturerService)
        {
            _claimService = claimService;
            _lecturerService = lecturerService;
        }

        public byte[] GenerateClaimsReport(IEnumerable<Claim> claims)
        {
            var reportContent = $"CLAIMS REPORT\nGenerated on: {DateTime.Now:yyyy-MM-dd HH:mm}\n\n";
            reportContent += "================================================================================\n";
            reportContent += "ID | Lecturer | Month | Year | Hours | Rate | Amount | Status | Submitted\n";
            reportContent += "================================================================================\n";

            foreach (var claim in claims)
            {
                reportContent += $"{claim.ClaimId} | Lecturer {claim.LecturerId} | {claim.Month} | {claim.Year} | " +
                               $"{claim.HoursWorked} | ${claim.HourlyRate} | ${claim.ClaimAmount} | " +
                               $"{claim.Status} | {claim.SubmissionDate:yyyy-MM-dd}\n";
            }

            reportContent += "================================================================================\n";
            reportContent += $"Total Claims: {claims.Count()}\n";
            reportContent += $"Total Amount: ${claims.Sum(c => c.ClaimAmount)}\n";

            return System.Text.Encoding.UTF8.GetBytes(reportContent);
        }

        public IEnumerable<Claim> GetClaimsReport(DateTime? startDate, DateTime? endDate, string? status)
        {
            var claims = _claimService.GetAllClaims();

            if (startDate.HasValue)
                claims = claims.Where(c => c.SubmissionDate >= startDate.Value);

            if (endDate.HasValue)
                claims = claims.Where(c => c.SubmissionDate <= endDate.Value);

            if (!string.IsNullOrEmpty(status))
                claims = claims.Where(c => c.Status == status);

            return claims;
        }

        public IEnumerable<Lecturer> GetLecturersReport()
        {
            return _lecturerService.GetAllLecturers();
        }

        public decimal GetTotalClaimsAmount(DateTime? startDate, DateTime? endDate)
        {
            var claims = GetClaimsReport(startDate, endDate, "Approved");
            return claims.Sum(c => c.ClaimAmount);
        }
    }
}