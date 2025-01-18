using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.TaskLabelEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services;

public class TaskLabelService : ITaskLabelService
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public TaskLabelService(IUserService userService, IMapper mapper, ApplicationDbContext context)
    {
        _userService = userService;
        _mapper = mapper;
        _context = context;
    }

    public async Task CreateAsync(TaskLabelCreate taskLabelCreate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();

        TaskLabel taskLabel = taskLabelCreate.ToTaskLabel(user.Id);

        await _context.TaskLabels.AddAsync(taskLabel);

        _context.SaveChangesAsync();
    }

    public async Task<ICollection<TaskLabel>> GetAllAsync(int userId)
    {
        return await _context.TaskLabels
            .Where(u => u.UserId == userId)
            .Include(u => u.User)
            .Include(u => u.StudyTasks)
            .ToListAsync();
    }

    public async Task UpdateAsync(TaskLabelUpdate taskLabelUpdate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) throw new Exception("User not found");

        TaskLabel taskLabel = await _context.TaskLabels.SingleAsync(u => u.Id == taskLabelUpdate.Id);

        taskLabel = taskLabelUpdate.ToTaskLabel(user.Id, taskLabel);

        _context.TaskLabels.Update(taskLabel);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        TaskLabel taskLabel = await _context.TaskLabels.SingleAsync(u => u.Id == id);

        _context.TaskLabels.Remove(taskLabel);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskLabel?> GetAsync(int id)
    {
        return await _context.TaskLabels.SingleAsync(u => u.Id == id);
    }
}
