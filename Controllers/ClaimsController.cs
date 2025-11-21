using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Contract_Monthly_Claim_System.Services;

namespace Contract_Monthly_Claim_System.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private readonly IClaimService _claimService;
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly ILecturerService _lecturerService;

        public ClaimsController(IClaimService claimService, IReportService reportService, IUserService userService, ILecturerService lecturerService)
        {
            _claimService = claimService;
            _reportService = reportService;
            _userService = userService;
            _lecturerService = lecturerService;
        }

        [Authorize(Roles = "Lecturer")]
        public IActionResult MyClaims()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var claims = _claimService.GetClaimsByLecturerId(userId);
            return View(claims);
        }

        [Authorize(Roles = "Lecturer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Lecturer")]
        public IActionResult Create(Models.Claim claim)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                claim.LecturerId = userId;
                claim.Status = "Pending";
                claim.SubmissionDate = DateTime.Now;
                claim.DateSubmitted = DateTime.Now;

                _claimService.AddClaim(claim);
                return RedirectToAction("MyClaims");
            }
            return View(claim);
        }

        [Authorize(Roles = "Admin,Coordinator")]
        public IActionResult Coordinator()
        {
            var claims = _claimService.GetAllClaims();
            return View(claims);
        }

        [Authorize(Roles = "Admin,HR")]
        public IActionResult HR()
        {
            var claims = _claimService.GetAllClaims();
            return View(claims);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Lecturer()
        {
            var claims = _claimService.GetAllClaims();
            return View(claims);
        }

        [Authorize(Roles = "Admin,Coordinator,HR")]
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var approvedBy = User.FindFirst("FullName")?.Value ?? "System";
            _claimService.ApproveClaim(id, approvedBy);
            return RedirectToAction("Coordinator");
        }

        [Authorize(Roles = "Admin,Coordinator,HR")]
        [HttpPost]
        public IActionResult Reject(int id, string reason)
        {
            var rejectedBy = User.FindFirst("FullName")?.Value ?? "System";
            _claimService.RejectClaim(id, rejectedBy, reason);
            return RedirectToAction("Coordinator");
        }

        [Authorize]
        public IActionResult Status(int id)
        {
            var claim = _claimService.GetClaimById(id);
            if (claim == null)
            {
                return NotFound();
            }
            return View(claim);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Reports()
        {
            var totalAmount = _reportService.GetTotalClaimsAmount(null, null);
            var pendingClaims = _claimService.GetClaimsByStatus("Pending").Count();
            var approvedClaims = _claimService.GetClaimsByStatus("Approved").Count();

            ViewBag.TotalAmount = totalAmount;
            ViewBag.PendingClaims = pendingClaims;
            ViewBag.ApprovedClaims = approvedClaims;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult GenerateReport(DateTime startDate, DateTime endDate, string reportType)
        {
            var claims = _reportService.GetClaimsReport(startDate, endDate, null);
            var reportService = new PdfReportService(_claimService, _lecturerService);
            var pdfBytes = reportService.GenerateClaimsReport(claims);

            return File(pdfBytes, "application/pdf", $"ClaimsReport_{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}