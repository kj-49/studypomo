﻿using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.StudyTypeEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Models.Tables.StudySessionEntities;

public class StudySessionCreate
{
    public string SessionUUID { get; set; }
    public int TotalPomodoros { get; set; }
    public long TotalFocusTime { get; set; }
    public long TotalBreakTime { get; set; }
}
