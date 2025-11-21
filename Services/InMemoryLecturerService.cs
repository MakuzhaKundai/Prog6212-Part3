using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Services
{
    public class InMemoryLecturerService : ILecturerService
    {
        private readonly List<Lecturer> _lecturers;

        public InMemoryLecturerService()
        {
            _lecturers = new List<Lecturer>
            {
                new Lecturer
                {
                    LecturerId = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@university.com",
                    Department = "Computer Science",
                    ContractType = "Part-Time",
                    HourlyRate = 50,
                    MonthlySalary = 0,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },
                new Lecturer
                {
                    LecturerId = 2,
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    Email = "sarah.johnson@university.com",
                    Department = "Mathematics",
                    ContractType = "Full-Time",
                    HourlyRate = 45,
                    MonthlySalary = 5000,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },
                new Lecturer
                {
                    LecturerId = 3,
                    FirstName = "Michael",
                    LastName = "Brown",
                    Email = "michael.brown@university.com",
                    Department = "Engineering",
                    ContractType = "Part-Time",
                    HourlyRate = 55,
                    MonthlySalary = 0,
                    IsActive = true,
                    DateCreated = DateTime.Now
                }
            };
        }

        public IEnumerable<Lecturer> GetAllLecturers()
        {
            return _lecturers.Where(l => l.IsActive);
        }

        public Lecturer? GetLecturerById(int id)
        {
            return _lecturers.FirstOrDefault(l => l.LecturerId == id && l.IsActive);
        }

        public void AddLecturer(Lecturer lecturer)
        {
            lecturer.LecturerId = _lecturers.Count + 1;
            lecturer.DateCreated = DateTime.Now;
            _lecturers.Add(lecturer);
        }

        public void UpdateLecturer(Lecturer lecturer)
        {
            var existingLecturer = GetLecturerById(lecturer.LecturerId);
            if (existingLecturer != null)
            {
                existingLecturer.FirstName = lecturer.FirstName;
                existingLecturer.LastName = lecturer.LastName;
                existingLecturer.Email = lecturer.Email;
                existingLecturer.Department = lecturer.Department;
                existingLecturer.ContractType = lecturer.ContractType;
                existingLecturer.HourlyRate = lecturer.HourlyRate;
                existingLecturer.MonthlySalary = lecturer.MonthlySalary;
                existingLecturer.IsActive = lecturer.IsActive;
            }
        }

        public void DeleteLecturer(int id)
        {
            var lecturer = GetLecturerById(id);
            if (lecturer != null)
            {
                lecturer.IsActive = false;
            }
        }

        public IEnumerable<Claim> GetClaimsByLecturerId(int lecturerId)
        {
            // This would typically get claims from a claim service
            // For now, return empty list - the claim service will handle this
            return new List<Claim>();
        }

        public bool LecturerExists(int id)
        {
            return _lecturers.Any(l => l.LecturerId == id && l.IsActive);
        }
    }
}