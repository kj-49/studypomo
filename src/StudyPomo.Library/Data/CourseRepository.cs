using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.CourseEntities;

namespace StudyPomo.Library.Data;

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
