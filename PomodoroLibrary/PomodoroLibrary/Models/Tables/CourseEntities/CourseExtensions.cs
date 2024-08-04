﻿using PomodoroLibrary.Models.Tables.CourseEntities;
using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.StudyTaskLabelEntities;
using PomodoroLibrary.Models.Tables.TaskLabelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables.CourseEntities;

public static class CourseExtensions
{
    public static Course ToEntity(this CourseCreate courseCreate, int userId)
    {
        Course course = new Course();

        course.UserId = userId;
        course.Name = courseCreate.Name;
        course.Description = courseCreate.Description;
        course.HexColor = courseCreate.HexColor;
        course.Archived = false;
        course.DateCreated = DateTime.UtcNow;
        course.DateUpdated = null;

        return course;
    }

    public static Course ToEntity(this CourseUpdate courseUpdate, Course? existingCourse = null)
    {
        if (existingCourse == null)
        {
            existingCourse = new Course();
        }

        existingCourse.Name = courseUpdate.Name;
        existingCourse.Description = courseUpdate.Description;
        existingCourse.HexColor = courseUpdate.HexColor;
        existingCourse.Archived = courseUpdate.Archived;
        existingCourse.DateUpdated = DateTime.UtcNow;

        return existingCourse;
    }
}
