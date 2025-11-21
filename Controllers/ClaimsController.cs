using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Contract_Monthly_Claim_System.Models;
using Microsoft.AspNetCore.Identity;
using Contract_Monthly_Claim_System.Data;

namespace Contract_Monthly_Claim_System.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClaimsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Claims/ReviewClaims
        [Authorize(Roles = "ProgrammeCoordinator,AcademicManager,HR")]
        public async Task<IActionResult> ReviewClaims()
        {
            var claims = await _context.Claims
                .Include(c => c.Lecturer)
                .OrderByDescending(c => c.DateSubmitted)
                .ToListAsync();

            return View(claims);
        }

        // GET: Claims/Reports
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Reports()
        {
            var claims = await _context.Claims
                .Include(c => c.Lecturer)
                .OrderByDescending(c => c.DateSubmitted)
                .ToListAsync();

            return View(claims);
        }

        // GET: Claims/Create
        [Authorize(Roles = "Lecturer")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Claims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> Create(Claim claim, IFormFile supportingDocument)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                claim.LecturerId = user.Id;
                claim.HourlyRate = user.HourlyRate;
                claim.TotalAmount = claim.HoursWorked * user.HourlyRate;
                claim.Status = "Pending";
                claim.DateSubmitted = DateTime.Now;

                // Handle file upload
                if (supportingDocument != null && supportingDocument.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + supportingDocument.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await supportingDocument.CopyToAsync(fileStream);
                    }

                    claim.DocumentPath = uniqueFileName;
                    claim.OriginalFileName = supportingDocument.FileName;
                }

                _context.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(claim);
        }

        // POST: Claims/Approve/5
        [HttpPost]
        [Authorize(Roles = "ProgrammeCoordinator,AcademicManager")]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            if (userRoles.Contains("ProgrammeCoordinator") && claim.Status == "Pending")
            {
                claim.Status = "ApprovedByCoordinator";
            }
            else if (userRoles.Contains("AcademicManager") && claim.Status == "ApprovedByCoordinator")
            {
                claim.Status = "Approved";
            }
            else if (userRoles.Contains("HR"))
            {
                claim.Status = "Approved";
            }

            claim.ProcessedBy = currentUser.Id;
            claim.DateProcessed = DateTime.Now;

            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ReviewClaims));
        }

        // POST: Claims/Reject/5
        [HttpPost]
        [Authorize(Roles = "ProgrammeCoordinator,AcademicManager,HR")]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            claim.Status = "Rejected";
            claim.ProcessedBy = currentUser.Id;
            claim.DateProcessed = DateTime.Now;

            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ReviewClaims));
        }

        // GET: Claims/DownloadDocument/5
        public async Task<IActionResult> DownloadDocument(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null || string.IsNullOrEmpty(claim.DocumentPath))
            {
                return NotFound();
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", claim.DocumentPath);
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(path), claim.OriginalFileName);
        }

        private string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
            {
                { ".pdf", "application/pdf" },
                { ".doc", "application/msword" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
            };
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }
    }
}