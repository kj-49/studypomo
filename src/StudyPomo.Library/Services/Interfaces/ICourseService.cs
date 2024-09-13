using StudyPomo.Library.Models.Tables.CourseEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;

namespace StudyPomo.Library.Services.Interfaces
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