using AutoMapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.StudyTaskLabelEntities;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;
using Pomodoro.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Services;

public class StudyTaskService : IStudyTaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public StudyTaskService(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task CreateAsync(StudyTaskCreate studyTaskCreate)
    {
            ApplicationUser? user = await _userService.GetCurrentUserAsync();
            if (user == null) throw new Exception("User not found");

        TaskPriority? taskPriority = await _unitOfWork.TaskPriority.GetAsync(u => u.Id == studyTaskCreate.TaskPriorityId);

        StudyTask studyTask = studyTaskCreate.ToEntity(user.Id);

        // Now add labels to task
        if (studyTaskCreate.TaskLabelIds != null)
        {
            foreach (int labelId in studyTaskCreate.TaskLabelIds)
            {
                var label = await _unitOfWork.TaskLabel.GetAsync(u => u.Id == labelId);
                if (label != null)
                {
                    await _unitOfWork.StudyTaskLabel.AddAsync(new StudyTaskLabel
                    {
                        StudyTask = studyTask,
                        TaskLabel = label
                    });
                }
            }
        }

        await _unitOfWork.StudyTask.AddAsync(studyTask);

        _unitOfWork.Complete();
    }

    public async Task RemoveAsync(int id)
    {
        StudyTask? studyTask = await _unitOfWork.StudyTask.GetAsync(u => u.Id == id);
        if (studyTask == null) throw new Exception("Study Task not found");

        _unitOfWork.StudyTask.Remove(studyTask);
        _unitOfWork.Complete();
    }

    public async Task ArchiveAsync(int id)
    {
        StudyTask? studyTask = await _unitOfWork.StudyTask.GetAsync(u => u.Id == id);
        if (studyTask == null) throw new Exception("Study Task not found");
        studyTask.Archived = true;

        _unitOfWork.StudyTask.Update(studyTask);
        _unitOfWork.Complete();
    }

    public async Task UpdateAsync(StudyTaskUpdate studyTaskUpdate)
    {
        StudyTask? studyTask = await _unitOfWork.StudyTask.GetAsync(u => u.Id == studyTaskUpdate.Id);

        if (studyTask == null) throw new Exception("Study Task not found");

        StudyTask updatedStudyTask = studyTaskUpdate.ToEntity(studyTask);

        await RemoveAllLabels(studyTask.Id);

        // Now add labels to task
        if (studyTaskUpdate.TaskLabelIds != null)
        {
            foreach (int labelId in studyTaskUpdate.TaskLabelIds)
            {
                var label = await _unitOfWork.TaskLabel.GetAsync(u => u.Id == labelId);
                if (label != null)
                {
                    await _unitOfWork.StudyTaskLabel.AddAsync(new StudyTaskLabel
                    {
                        StudyTask = studyTask,
                        TaskLabel = label
                    });
                }
            }
        }

        _unitOfWork.StudyTask.Update(updatedStudyTask);
        _unitOfWork.Complete();
    }

    public async Task RemoveAllLabels(int studyTaskId)
    
    {
        IEnumerable<StudyTaskLabel> studyTaskLabels = await _unitOfWork.StudyTaskLabel.GetAllAsync(u => u.StudyTask.Id == studyTaskId);

        _unitOfWork.StudyTaskLabel.RemoveRange(studyTaskLabels);

        _unitOfWork.Complete();
    }

    public async Task CompleteAsync(int id)
    {
        StudyTask? studyTask = await _unitOfWork.StudyTask.GetAsync(u => u.Id == id);

        if (studyTask == null) throw new Exception("Study Task not found");

        studyTask.Completed = true;
        studyTask.DateCompleted = DateTime.UtcNow;

        _unitOfWork.StudyTask.Update(studyTask);
        _unitOfWork.Complete();
    }

    public async Task UncompleteAsync(int id)
    {
        StudyTask? studyTask = await _unitOfWork.StudyTask.GetAsync(u => u.Id == id);

        if (studyTask == null) throw new Exception("Study Task not found");

        studyTask.Completed = false;
        studyTask.DateCompleted = null;

        _unitOfWork.StudyTask.Update(studyTask);
        _unitOfWork.Complete();
    }

    public async Task<ICollection<StudyTask>> GetAllAsync(int userId)
    {
        IEnumerable<StudyTask> studyTasks = await _unitOfWork.StudyTask.GetAllAsync(
            u => u.User.Id == userId,
            t => t.TaskPriority,
            t => t.User,
            t => t.TaskLabels
        );

        return studyTasks.ToList();
    }

    public async Task<StudyTask> GetAsync(int id)
    {
        StudyTask? studyTask = await _unitOfWork.StudyTask.GetAsync(
            u => u.Id == id,
            t => t.TaskLabels
        );

        if (studyTask == null)
        {
            throw new Exception("Study Task not found");
        }

        return studyTask;
    }
}
