using StudyPomo.Library.Models.Tables.StudySessionEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;

namespace StudyPomo.Library.Services.Interfaces
{
    public interface IStudySessionService
    {
        Task CreateAsync(StudySessionCreate studySessionCreate);
        Task<ICollection<StudySession>> GetAllAsync(int userId);
        Task UpdateAsync(StudySessionUpdate studySessionUpdate);
        Task<StudySession?> GetAsync(string UUID);
    }
}