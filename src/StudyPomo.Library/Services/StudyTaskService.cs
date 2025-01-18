using AutoMapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.StudyTaskLabelEntities;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;
using StudyPomo.Library.Models.Utility;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services;

public class StudyTaskService : IStudyTaskService
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;

    public StudyTaskService(IMapper mapper, IUserService userService, ApplicationDbContext context)
    {
        _mapper = mapper;
        _userService = userService;
        _context = context;
    }

    public async Task CreateAsync(StudyTaskCreate studyTaskCreate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) throw new Exception("User not found");

        StudyTask studyTask = studyTaskCreate.ToEntity(user.Id, TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId ?? SD.UTC));

        // Now add labels to task
        if (studyTaskCreate.TaskLabelIds != null && studyTaskCreate.TaskLabelIds.Any())
        {
            // Fetch all labels at once
            var labels = await _context.TaskLabels.Where(u => studyTaskCreate.TaskLabelIds.Contains(u.Id)).ToListAsync();

            // Create the StudyTaskLabel entities
            var studyTaskLabels = labels.Select(label => new StudyTaskLabel
            {
                StudyTask = studyTask,
                TaskLabel = label
            });

            // Add all StudyTaskLabels in one go
            await _context.StudyTaskLabels.AddRangeAsync(studyTaskLabels);
        }

        await _context.StudyTasks.AddAsync(studyTask);

        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        StudyTask? studyTask = await _context.StudyTasks.SingleAsync(u => u.Id == id);
        if (studyTask == null) throw new Exception("Study Task not found");

        _context.StudyTasks.Remove(studyTask);
        await _context.SaveChangesAsync();
    }

    public async Task ArchiveAsync(int id)
    {
        StudyTask? studyTask = await _context.StudyTasks.SingleAsync(u => u.Id == id);
        if (studyTask == null) throw new Exception("Study Task not found");
        studyTask.Archived = true;

        _context.StudyTasks.Update(studyTask);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StudyTaskUpdate studyTaskUpdate)
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        StudyTask? studyTask = await _context.StudyTasks.SingleAsync(u => u.Id == studyTaskUpdate.Id);
        if (studyTask == null) throw new Exception("Study Task not found");

        StudyTask updatedStudyTask = studyTaskUpdate.ToEntity(TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId ?? SD.UTC), studyTask);

        await RemoveAllLabels(studyTask.Id);

        // Now add labels to task
        if (studyTaskUpdate.TaskLabelIds != null && studyTaskUpdate.TaskLabelIds.Any())
        {
            var labels = await _context.TaskLabels.Where(u => studyTaskUpdate.TaskLabelIds.Contains(u.Id)).ToListAsync();

            var studyTaskLabels = labels.Select(label => new StudyTaskLabel
            {
                StudyTaskId = studyTask.Id,
                TaskLabelId = label.Id
            });

            await _context.StudyTaskLabels.AddRangeAsync(studyTaskLabels);
        }

        _context.StudyTasks.Update(updatedStudyTask);
        await _context.SaveChangesAsync();
    }


    public async Task RemoveAllLabels(int studyTaskId)
    
    {
        IEnumerable<StudyTaskLabel> studyTaskLabels = await _context.StudyTaskLabels.Where(u => u.StudyTask.Id == studyTaskId).ToListAsync();

        _context.StudyTaskLabels.RemoveRange(studyTaskLabels);

        await _context.SaveChangesAsync();
    }

    public async Task CompleteAsync(int id)
    {
        StudyTask? studyTask = await _context.StudyTasks.SingleAsync(u => u.Id == id);

        studyTask.Completed = true;
        studyTask.DateCompleted = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task UncompleteAsync(int id)
    {
        StudyTask studyTask = await _context.StudyTasks.SingleAsync(u => u.Id == id);

        studyTask.Completed = false;
        studyTask.DateCompleted = null;

        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<StudyTask>> GetAllAsync(int userId, bool includeArchived = false, bool showFromArchivedCourses = false)
    {
        IQueryable<StudyTask> query = _context.StudyTasks;

        if (!includeArchived)
        {
            query = query.Where(u => !u.Archived);
        }

        if (!showFromArchivedCourses)
        {
            query = query.Where(u => u.Course == null || !u.Course.Archived);
        }

        return await query
            .Include(u => u.TaskPriority)
            .Include(u => u.User)
            .Include(u => u.TaskLabels)
            .Include(u => u.TaskLabels)
            .Include(u => u.Course)
            .Where(u => u.UserId == userId)
            .ToListAsync();
    }

    public async Task<StudyTask> GetAsync(int id)
    {
        return await _context.StudyTasks
            .Include(u => u.TaskLabels)
            .SingleAsync(u => u.Id == id);
    }
}
