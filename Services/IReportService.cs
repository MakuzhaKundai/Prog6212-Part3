using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Services
{
    public interface IReportService
    {
        IEnumerable<Claim> GetClaimsReport(DateTime? startDate, DateTime? endDate, string? status);
        IEnumerable<Lecturer> GetLecturersReport();
        decimal GetTotalClaimsAmount(DateTime? startDate, DateTime? endDate);
    }
}