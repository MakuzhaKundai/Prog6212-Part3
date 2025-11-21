using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Contract_Monthly_Claim_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Contract_Monthly_Claim_System.Controllers
{
    [Authorize(Roles = "HR")]
    public class LecturerManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public LecturerManagementController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: LecturerManagement
        public async Task<IActionResult> Index()
        {
            // FIX: Get only users with Lecturer role
            var lecturers = await _userManager.GetUsersInRoleAsync("Lecturer");
            return View(lecturers.ToList());
        }
    }
}