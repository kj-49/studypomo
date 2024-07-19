using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Tables.TaskPriorityEntities;
using PomodoroLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Services;

public class TaskPriorityService : ITaskPriorityService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskPriorityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ICollection<TaskPriority>> GetAllAsync()
    {
        IEnumerable<TaskPriority> taskPriorities = await _unitOfWork.TaskPriority.GetAllAsync();
        return taskPriorities.ToList();
    }
}
