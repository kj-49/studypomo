using Pomodoro.Library.Data.Database;
using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Data
{
    public class TaskLabelRepository : GeneralRepository<TaskLabel>, ITaskLabelRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskLabelRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TaskLabel model)
        {
            _db.Update(model);
        }
    }
}
