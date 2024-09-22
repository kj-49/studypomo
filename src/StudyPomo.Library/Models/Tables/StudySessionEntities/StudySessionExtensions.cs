using Humanizer;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.StudyTaskLabelEntities;
using StudyPomo.Library.Models.Tables.TaskLabelEntities;
using StudyPomo.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Models.Tables.StudySessionEntities;

public static class StudySessionExtensions
{
    public static StudySession ToEntity(this StudySessionCreate studySessionCreate, int userId)
    {
        StudySession studySession = new StudySession();
        studySession.UserId = userId;
        studySession.SessionUUID = studySessionCreate.SessionUUID;
        studySession.DateStarted = DateTime.UtcNow;
        studySession.TotalPomodoros = studySessionCreate.TotalPomodoros;
        studySession.TotalFocusTime = studySessionCreate.TotalFocusTime;
        studySession.TotalBreakTime = studySessionCreate.TotalBreakTime;

        return studySession;
    }

    public static StudySession ToEntity(this StudySessionUpdate studyTaskUpdate, StudySession? exisitingStudySession = null)
    {
        if (exisitingStudySession == null)
        {
            exisitingStudySession = new StudySession();
        }

        exisitingStudySession.TotalPomodoros = studyTaskUpdate.TotalPomodoros;
        exisitingStudySession.TotalFocusTime = studyTaskUpdate.TotalFocusTime;
        exisitingStudySession.TotalBreakTime = studyTaskUpdate.TotalBreakTime;
        exisitingStudySession.DateModified = DateTime.UtcNow;

        return exisitingStudySession;
    }
}
