using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using StudyPomo.Library.Models.Tables.StudySessionEntities;

namespace StudyPomo.Library.Data;

public class StudySessionRepository : GeneralRepository<StudySession>, IStudySessionRepository
{
    private readonly ApplicationDbContext _db;

    public StudySessionRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(StudySession model)
    {
        _db.Update(model);
    }
}
