using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Tables.StudyTaskLabelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Data;

public class StudyTaskLabelRepository : GeneralRepository<StudyTaskLabel>, IStudyTaskLabelRepository
{
    private readonly ApplicationDbContext _db;

    public StudyTaskLabelRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}
