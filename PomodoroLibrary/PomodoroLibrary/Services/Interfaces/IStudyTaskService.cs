using PomodoroLibrary.Models.Tables.StudyTaskEntities;

namespace PomodoroLibrary.Services.Interfaces
{
    public interface IStudyTaskService
    {
        Task CreateAsync(StudyTaskVM studyTaskCreate);
        Task RemoveAsync(int id);
    }
}