using Pomodoro.Library.Models.Tables.StudyTaskEntities;

namespace Pomodoro.Library.Services.Interfaces
{
    public interface IStudyTaskService
    {
        Task CreateAsync(StudyTaskCreate studyTaskCreate);
        Task RemoveAsync(int id);
        Task UpdateAsync(StudyTaskUpdate studyTaskUpdate);
        Task CompleteAsync(int id);
        Task ArchiveAsync(int id);
        Task UncompleteAsync(int id);
        Task<ICollection<StudyTask>> GetAllAsync(int userId);
        Task<StudyTask> GetAsync(int id);
    }
}