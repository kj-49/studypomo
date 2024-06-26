using PomodoroLibrary.Data.Database;
using PomodoroLibrary.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data;

public class UnitOfWork : IUnitOfWork
{
    public IStudyTaskRepository StudyTask { get; private set; }

    private ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        StudyTask = new StudyTaskRepository(_db);
    }

    public void Save()
    {
        _db.SaveChanges();
    }

}
