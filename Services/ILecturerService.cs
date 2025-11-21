using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Services
{
    public interface ILecturerService
    {
        IEnumerable<Lecturer> GetAllLecturers();
        Lecturer? GetLecturerById(int id);
        void AddLecturer(Lecturer lecturer);
        void UpdateLecturer(Lecturer lecturer);
        void DeleteLecturer(int id);
        IEnumerable<Claim> GetClaimsByLecturerId(int lecturerId);
        bool LecturerExists(int id);
    }
}