using Microsoft.EntityFrameworkCore;
using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services;

public class TaskPriorityService : ITaskPriorityService
{
    private readonly ApplicationDbContext _context;

    public TaskPriorityService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<TaskPriority>> GetAllAsync()
    {
        return await _context.TaskPriorities.ToListAsync();
    }
}
