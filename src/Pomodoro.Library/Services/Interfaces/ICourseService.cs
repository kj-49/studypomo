using Pomodoro.Library.Models.Tables.CourseEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;

namespace Pomodoro.Library.Services.Interfaces
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