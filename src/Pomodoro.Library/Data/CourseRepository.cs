using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Data.Database;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.CourseEntities;

namespace Pomodoro.Library.Data;

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
