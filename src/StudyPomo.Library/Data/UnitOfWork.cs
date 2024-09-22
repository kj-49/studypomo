using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Data;

public class UnitOfWork : IUnitOfWork
{
    public IStudyTaskRepository StudyTask { get; private set; }
    public ITaskPriorityRepository TaskPriority { get; private set; }
    public IUserRepository User { get; private set; }
    public ITaskLabelRepository TaskLabel { get; private set; }
    public IStudyTaskLabelRepository StudyTaskLabel { get; private set; }
    public ICourseRepository Course { get; private set; }

    public IStudySessionRepository StudySession { get; private set; }

    private ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        StudyTask = new StudyTaskRepository(_db);
        TaskPriority = new TaskPriorityRepository(_db);
        User = new UserRepository(_db);
        TaskLabel = new TaskLabelRepository(_db);
        StudyTaskLabel = new StudyTaskLabelRepository(_db);
        Course = new CourseRepository(_db);
        StudySession = new StudySessionRepository(_db);
    }

    public int Complete()
    {
        return _db.SaveChanges();
    }

}
