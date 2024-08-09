using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Data.Database;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;

namespace Pomodoro.Library.Data;

public class StudyTaskRepository : GeneralRepository<StudyTask>, IStudyTaskRepository
{
    private readonly ApplicationDbContext _db;

    public StudyTaskRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(StudyTask model)
    {
        _db.Update(model);
    }
}
