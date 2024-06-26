using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Services;

public class StudyTaskService
{
    private readonly IStudyTaskRepository _studyTaskRepository;

    public StudyTaskService(IStudyTaskRepository studyTaskRepository)
    {
        _studyTaskRepository = studyTaskRepository;
    }

    public async Task CreateAsync(StudyTaskCreate studyTaskCreate)
    {
        //StudyTask studyTask - new StudyTask
        //{

        //}

        //_studyTaskRepository.AddAsync
    }

}
