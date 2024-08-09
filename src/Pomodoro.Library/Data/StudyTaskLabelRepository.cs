using Pomodoro.Library.Data.Database;
using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Models.Tables.StudyTaskLabelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Data;

public class StudyTaskLabelRepository : GeneralRepository<StudyTaskLabel>, IStudyTaskLabelRepository
{
    private readonly ApplicationDbContext _db;

    public StudyTaskLabelRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}
