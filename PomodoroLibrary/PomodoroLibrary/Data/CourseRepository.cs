using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Data.Database;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Models.Tables.CourseEntities;

namespace PomodoroLibrary.Data;

public class CourseRepository : GeneralRepository<Course>, ICourseRepository
{
    private readonly ApplicationDbContext _db;

    public CourseRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Course model)
    {
        _db.Update(model);
    }
}
