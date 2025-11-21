using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Contract_Monthly_Claim_System.Models;
using Contract_Monthly_Claim_System.Services;

namespace Contract_Monthly_Claim_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LecturerManagementController : Controller
    {
        private readonly ILecturerService _lecturerService;
        private readonly IUserService _userService;

        public LecturerManagementController(ILecturerService lecturerService, IUserService userService)
        {
            _lecturerService = lecturerService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var lecturers = _lecturerService.GetAllLecturers();
            return View(lecturers);
        }

        public IActionResult Details(int id)
        {
            var lecturer = _lecturerService.GetLecturerById(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _lecturerService.AddLecturer(lecturer);

                // Also create a user account for the lecturer
                var user = new User
                {
                    Username = lecturer.Email.Split('@')[0],
                    Password = "password123", // Default password
                    Role = "Lecturer",
                    Email = lecturer.Email,
                    FirstName = lecturer.FirstName,
                    LastName = lecturer.LastName,
                    HourlyRate = lecturer.HourlyRate,
                    IsActive = true
                };
                _userService.AddUser(user);

                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

        public IActionResult Edit(int id)
        {
            var lecturer = _lecturerService.GetLecturerById(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Lecturer lecturer)
        {
            if (id != lecturer.LecturerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _lecturerService.UpdateLecturer(lecturer);
                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

        public IActionResult Delete(int id)
        {
            var lecturer = _lecturerService.GetLecturerById(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _lecturerService.DeleteLecturer(id);
            return RedirectToAction(nameof(Index));
        }
    }
}