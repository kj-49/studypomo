using AutoMapper;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StudyTaskVM, StudyTask>();
    }
}
