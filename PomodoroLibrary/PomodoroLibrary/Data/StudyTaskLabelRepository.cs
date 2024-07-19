using PomodoroLibrary.Data.Database;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Tables.StudyTaskLabelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data;

public class StudyTaskLabelRepository : GeneralRepository<StudyTaskLabel>, IStudyTaskLabelRepository
{
    private readonly ApplicationDbContext _db;

    public StudyTaskLabelRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}
