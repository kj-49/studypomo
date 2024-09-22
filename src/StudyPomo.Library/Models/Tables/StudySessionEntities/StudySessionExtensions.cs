using Humanizer;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using StudyPomo.Library.Models.Tables.LabelEntities;
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
}
