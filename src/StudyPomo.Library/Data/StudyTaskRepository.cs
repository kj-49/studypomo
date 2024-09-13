using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace StudyPomo.Library.Data;

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
