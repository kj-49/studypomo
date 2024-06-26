using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Data.Database;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;

namespace PomodoroLibrary.Data;

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
