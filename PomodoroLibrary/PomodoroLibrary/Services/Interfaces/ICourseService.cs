using PomodoroLibrary.Models.Tables.CourseEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;

namespace PomodoroLibrary.Services.Interfaces
{
    public interface ICourseService
    {
        Task CreateAsync(CourseCreate courseCreate);
        Task RemoveAsync(int id);
        Task UpdateAsync(CourseUpdate courseUpdate);
        Task ArchiveAsync(int id);
        Task<ICollection<Course>> GetAllAsync(int userId);
        Task<Course> GetAsync(int id);
    }
}