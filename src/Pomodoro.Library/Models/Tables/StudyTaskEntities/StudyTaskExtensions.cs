using Humanizer;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskLabelEntities;
using Pomodoro.Library.Models.Tables.TaskLabelEntities;
using Pomodoro.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Models.Tables.StudyTaskEntities;

public static class StudyTaskExtensions
{
    public static StudyTask ToEntity(this StudyTaskCreate studyTaskCreate, int userId, TimeZoneInfo userTimeZone)
    {
        StudyTask studyTask = new StudyTask();

        studyTask.UserId = userId;
        studyTask.Name = studyTaskCreate.Name;
        studyTask.Completed = false;
        studyTask.DateCreated = DateTime.UtcNow;
        studyTask.DateCompleted = null;
        studyTask.TaskPriorityId = studyTaskCreate.TaskPriorityId;
        studyTask.Archived = false;
        studyTask.Deadline = studyTaskCreate.Deadline == null ? null : TimeService.ConvertFromUserTime(studyTaskCreate.Deadline.Value, userTimeZone);
        studyTask.CourseId = studyTaskCreate.CourseId;

        return studyTask;
    }

    public static StudyTask ToEntity(this StudyTaskUpdate studyTaskUpdate, TimeZoneInfo userTimeZone, StudyTask? existingStudyTask = null)
    {
        if (existingStudyTask == null)
        {
            existingStudyTask = new StudyTask();
        }

        existingStudyTask.Name = studyTaskUpdate.Name;
        existingStudyTask.TaskPriorityId = studyTaskUpdate.TaskPriorityId;
        existingStudyTask.Deadline = studyTaskUpdate.Deadline == null ? null : TimeService.ConvertFromUserTime(studyTaskUpdate.Deadline.Value, userTimeZone);
        existingStudyTask.CourseId = studyTaskUpdate.CourseId;

        return existingStudyTask;
    }

    public static string Status(this StudyTask studyTask)
    {
        if (studyTask.Completed)
        {
            return $"Completed {studyTask.DateCompleted.Humanize()}";
        } else
        {
            return $"Created {studyTask.DateCreated.Humanize()}";
        }
    }

    /// <summary>
    /// Get the next StudyTasks to be completed.
    /// </summary>
    /// <param name="studyTasks"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static IEnumerable<StudyTask> Next(this IEnumerable<StudyTask> studyTasks, int amount,bool includeArchived = false)
    {
        if (!includeArchived)
        {
            studyTasks = studyTasks.Where(t => !t.Archived && !t.Completed);
        }

        return studyTasks
            .OrderBy(t => t.Deadline == null)
            .ThenBy(t => t.Deadline)
            .Take(amount);
    }
}
