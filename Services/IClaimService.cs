using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Services
{
    public interface IClaimService
    {
        IEnumerable<Claim> GetAllClaims();
        Claim? GetClaimById(int id);
        void AddClaim(Claim claim);
        void UpdateClaim(Claim claim);
        void DeleteClaim(int id);
        IEnumerable<Claim> GetClaimsByLecturerId(int lecturerId);
        IEnumerable<Claim> GetClaimsByStatus(string status);
        void ApproveClaim(int claimId, string approvedBy);
        void RejectClaim(int claimId, string rejectedBy, string reason);
        bool ClaimExists(int id);
    }
}