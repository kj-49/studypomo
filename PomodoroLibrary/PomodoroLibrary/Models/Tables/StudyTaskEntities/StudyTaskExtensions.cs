using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.StudyTaskLabelEntities;
using PomodoroLibrary.Models.Tables.TaskLabelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables.StudyTaskEntities;

public static class StudyTaskExtensions
{
    public static StudyTask ToEntity(this StudyTaskCreate studyTaskCreate, int userId)
    {
        StudyTask studyTask = new StudyTask();

        studyTask.UserId = userId;
        studyTask.Name = studyTaskCreate.Name;
        studyTask.Completed = false;
        studyTask.DateCreated = DateTime.UtcNow;
        studyTask.DateCompleted = null;
        studyTask.TaskPriorityId = studyTaskCreate.TaskPriorityId;
        studyTask.Archived = false;
        studyTask.Deadline = studyTaskCreate.Deadline;

        return studyTask;
    }

    public static StudyTask ToEntity(this StudyTaskUpdate studyTaskUpdate, StudyTask? existingStudyTask = null)
    {
        if (existingStudyTask == null)
        {
            existingStudyTask = new StudyTask();
        }

        existingStudyTask.Name = studyTaskUpdate.Name;
        existingStudyTask.TaskPriorityId = studyTaskUpdate.TaskPriorityId;
        existingStudyTask.Deadline = studyTaskUpdate.Deadline;

        return existingStudyTask;
    }
}
