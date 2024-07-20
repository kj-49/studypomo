using AutoMapper;
using PomodoroLibrary.Models.Tables.LabelEntities;
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
        CreateMap<StudyTask, StudyTaskUpdate>()
            .ForMember(dest => dest.TaskLabelIds, opt => opt.MapFrom(src => src.TaskLabels.Select(label => label.Id).ToList()))
            .ReverseMap()
            .ForMember(dest => dest.TaskLabels, opt => opt.MapFrom(src => src.TaskLabelIds.Select(id => new TaskLabel { Id = id }).ToList()));

    }
}
