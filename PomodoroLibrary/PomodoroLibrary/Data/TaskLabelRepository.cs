using PomodoroLibrary.Data.Database;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Models.Tables.TaskPriorityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data
{
    public class TaskLabelRepository : GeneralRepository<TaskLabel>, ITaskLabelRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskLabelRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(TaskLabel model)
        {
            _db.Update(model);
        }
    }
}
