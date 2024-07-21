using PomodoroLibrary.Models.Tables.LabelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables.TaskLabelEntities;

public static class TaskLabelExtensions
{
    public static TaskLabel ToTaskLabel(this TaskLabelUpdate taskLabelUpdate, int userId, TaskLabel existingTaskLabel = null)
    {
        if (existingTaskLabel == null)
        {
            existingTaskLabel = new TaskLabel();
        }

        existingTaskLabel.Name = taskLabelUpdate.Name;
        existingTaskLabel.Description = taskLabelUpdate.Description;
        existingTaskLabel.HexColor = taskLabelUpdate.HexColor;
        existingTaskLabel.UserId = userId;

        return existingTaskLabel;
    }

    public static TaskLabel ToTaskLabel(this TaskLabelCreate taskLabelCreate, int userId, TaskLabel existingTaskLabel = null)
    {
        if (existingTaskLabel == null)
        {
            existingTaskLabel = new TaskLabel();
        }

        existingTaskLabel.Name = taskLabelCreate.Name;
        existingTaskLabel.Description = taskLabelCreate.Description;
        existingTaskLabel.HexColor = taskLabelCreate.HexColor;
        existingTaskLabel.UserId = userId;

        return existingTaskLabel;
    }
}
