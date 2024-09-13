using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Data
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
