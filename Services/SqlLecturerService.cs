using Contract_Monthly_Claim_System.Models;
using Contract_Monthly_Claim_System.Data;
using Microsoft.EntityFrameworkCore;

namespace Contract_Monthly_Claim_System.Services
{
    public class SqlLecturerService : ILecturerService
    {
        private readonly ApplicationDbContext _context;

        public SqlLecturerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Lecturer> GetAllLecturers()
        {
            return _context.Lecturers.Where(l => l.IsActive).ToList();
        }

        public Lecturer? GetLecturerById(int id)
        {
            return _context.Lecturers.FirstOrDefault(l => l.LecturerId == id && l.IsActive);
        }

        public void AddLecturer(Lecturer lecturer)
        {
            lecturer.DateCreated = DateTime.Now;
            _context.Lecturers.Add(lecturer);
            _context.SaveChanges();
        }

        public void UpdateLecturer(Lecturer lecturer)
        {
            _context.Lecturers.Update(lecturer);
            _context.SaveChanges();
        }

        public void DeleteLecturer(int id)
        {
            var lecturer = GetLecturerById(id);
            if (lecturer != null)
            {
                lecturer.IsActive = false;
                _context.SaveChanges();
            }
        }

        public IEnumerable<Claim> GetClaimsByLecturerId(int lecturerId)
        {
            return _context.Claims.Where(c => c.LecturerId == lecturerId).ToList();
        }

        public bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(l => l.LecturerId == id && l.IsActive);
        }
    }
}