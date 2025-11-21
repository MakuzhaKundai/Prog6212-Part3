using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Services
{
    public class InMemoryClaimService : IClaimService
    {
        private readonly List<Claim> _claims;
        private int _nextId = 1;

        public InMemoryClaimService()
        {
            _claims = new List<Claim>
            {
                new Claim
                {
                    ClaimId = _nextId++,
                    LecturerId = 1,
                    Month = "January",
                    Year = 2024,
                    HoursWorked = 40,
                    HourlyRate = 50,
                    Status = "Approved",
                    SubmissionDate = DateTime.Now.AddDays(-30),
                    DateSubmitted = DateTime.Now.AddDays(-30),
                    ApprovalDate = DateTime.Now.AddDays(-25),
                    DateApproved = DateTime.Now.AddDays(-25),
                    ApprovedBy = "Admin",
                    Notes = "Regular monthly claim",
                    Documents = "timesheet.pdf"
                },
                new Claim
                {
                    ClaimId = _nextId++,
                    LecturerId = 2,
                    Month = "February",
                    Year = 2024,
                    HoursWorked = 35,
                    HourlyRate = 45,
                    Status = "Pending",
                    SubmissionDate = DateTime.Now.AddDays(-10),
                    DateSubmitted = DateTime.Now.AddDays(-10),
                    Notes = "Additional tutoring hours",
                    Documents = "tutoring_schedule.pdf"
                },
                new Claim
                {
                    ClaimId = _nextId++,
                    LecturerId = 1,
                    Month = "March",
                    Year = 2024,
                    HoursWorked = 42,
                    HourlyRate = 50,
                    Status = "Rejected",
                    SubmissionDate = DateTime.Now.AddDays(-5),
                    DateSubmitted = DateTime.Now.AddDays(-5),
                    ApprovalDate = DateTime.Now.AddDays(-2),
                    DateApproved = DateTime.Now.AddDays(-2),
                    ApprovedBy = "Coordinator",
                    RejectionReason = "Insufficient documentation",
                    Notes = "Research supervision",
                    Documents = "research_log.pdf"
                }
            };
        }

        public IEnumerable<Claim> GetAllClaims()
        {
            return _claims.OrderByDescending(c => c.SubmissionDate);
        }

        public Claim? GetClaimById(int id)
        {
            return _claims.FirstOrDefault(c => c.ClaimId == id);
        }

        public void AddClaim(Claim claim)
        {
            claim.ClaimId = _nextId++;
            claim.SubmissionDate = DateTime.Now;
            claim.DateSubmitted = DateTime.Now;
            claim.Status = "Pending";
            _claims.Add(claim);
        }

        public void UpdateClaim(Claim claim)
        {
            var existingClaim = GetClaimById(claim.ClaimId);
            if (existingClaim != null)
            {
                existingClaim.Month = claim.Month;
                existingClaim.Year = claim.Year;
                existingClaim.HoursWorked = claim.HoursWorked;
                existingClaim.HourlyRate = claim.HourlyRate;
                existingClaim.Notes = claim.Notes;
                existingClaim.Documents = claim.Documents;
                existingClaim.Status = claim.Status;

                if (claim.Status == "Approved" && existingClaim.Status != "Approved")
                {
                    existingClaim.ApprovalDate = DateTime.Now;
                    existingClaim.DateApproved = DateTime.Now;
                    existingClaim.ApprovedBy = claim.ApprovedBy;
                }
                else if (claim.Status == "Rejected")
                {
                    existingClaim.ApprovalDate = DateTime.Now;
                    existingClaim.DateApproved = DateTime.Now;
                    existingClaim.ApprovedBy = claim.ApprovedBy;
                    existingClaim.RejectionReason = claim.RejectionReason;
                }
            }
        }

        public void DeleteClaim(int id)
        {
            var claim = GetClaimById(id);
            if (claim != null)
            {
                _claims.Remove(claim);
            }
        }

        public IEnumerable<Claim> GetClaimsByLecturerId(int lecturerId)
        {
            return _claims.Where(c => c.LecturerId == lecturerId)
                         .OrderByDescending(c => c.SubmissionDate);
        }

        public IEnumerable<Claim> GetClaimsByStatus(string status)
        {
            return _claims.Where(c => c.Status == status)
                         .OrderByDescending(c => c.SubmissionDate);
        }

        public void ApproveClaim(int claimId, string approvedBy)
        {
            var claim = GetClaimById(claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
                claim.ApprovalDate = DateTime.Now;
                claim.DateApproved = DateTime.Now;
                claim.ApprovedBy = approvedBy;
                claim.RejectionReason = null;
            }
        }

        public void RejectClaim(int claimId, string rejectedBy, string reason)
        {
            var claim = GetClaimById(claimId);
            if (claim != null)
            {
                claim.Status = "Rejected";
                claim.ApprovalDate = DateTime.Now;
                claim.DateApproved = DateTime.Now;
                claim.ApprovedBy = rejectedBy;
                claim.RejectionReason = reason;
            }
        }

        public bool ClaimExists(int id)
        {
            return _claims.Any(c => c.ClaimId == id);
        }
    }
}