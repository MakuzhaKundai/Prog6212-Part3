using Contract_Monthly_Claim_System.Models;
using Contract_Monthly_Claim_System.Data;

namespace Contract_Monthly_Claim_System.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Claim> GetClaimsReport(DateTime? startDate, DateTime? endDate, string? status)
        {
            var query = _context.Claims.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(c => c.SubmissionDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.SubmissionDate <= endDate.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            return query.ToList();
        }

        public IEnumerable<Lecturer> GetLecturersReport()
        {
            return _context.Lecturers.Where(l => l.IsActive).ToList();
        }

        public decimal GetTotalClaimsAmount(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Claims.Where(c => c.Status == "Approved");

            if (startDate.HasValue)
                query = query.Where(c => c.SubmissionDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.SubmissionDate <= endDate.Value);

            return query.Sum(c => c.ClaimAmount);
        }
    }
}