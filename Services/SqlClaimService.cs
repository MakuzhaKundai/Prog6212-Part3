using Contract_Monthly_Claim_System.Models;
using Contract_Monthly_Claim_System.Data;
using Microsoft.EntityFrameworkCore;

namespace Contract_Monthly_Claim_System.Services
{
    public class SqlClaimService : IClaimService
    {
        private readonly ApplicationDbContext _context;

        public SqlClaimService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Claim> GetAllClaims()
        {
            return _context.Claims
                .Include(c => c.Lecturer)
                .ToList();
        }

        public Claim? GetClaimById(int id)
        {
            return _context.Claims
                .Include(c => c.Lecturer)
                .FirstOrDefault(c => c.ClaimId == id);
        }

        public void AddClaim(Claim claim)
        {
            claim.SubmissionDate = DateTime.Now;
            claim.Status = "Pending";
            _context.Claims.Add(claim);
            _context.SaveChanges();
        }

        public void UpdateClaim(Claim claim)
        {
            _context.Claims.Update(claim);
            _context.SaveChanges();
        }

        public void DeleteClaim(int id)
        {
            var claim = GetClaimById(id);
            if (claim != null)
            {
                _context.Claims.Remove(claim);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Claim> GetClaimsByLecturerId(int lecturerId)
        {
            return _context.Claims
                .Where(c => c.LecturerId == lecturerId)
                .Include(c => c.Lecturer)
                .ToList();
        }

        public IEnumerable<Claim> GetClaimsByStatus(string status)
        {
            return _context.Claims
                .Where(c => c.Status == status)
                .Include(c => c.Lecturer)
                .ToList();
        }

        public void ApproveClaim(int claimId, string approvedBy)
        {
            var claim = GetClaimById(claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
                claim.ApprovalDate = DateTime.Now;
                claim.ApprovedBy = approvedBy;
                _context.SaveChanges();
            }
        }

        public void RejectClaim(int claimId, string rejectedBy, string reason)
        {
            var claim = GetClaimById(claimId);
            if (claim != null)
            {
                claim.Status = "Rejected";
                claim.ApprovalDate = DateTime.Now;
                claim.ApprovedBy = rejectedBy;
                claim.RejectionReason = reason;
                _context.SaveChanges();
            }
        }

        public bool ClaimExists(int id)
        {
            return _context.Claims.Any(c => c.ClaimId == id);
        }
    }
}