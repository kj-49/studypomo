using AutoMapper;
using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Tables.TaskLabelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Models.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StudyTask, StudyTaskUpdate>()
            .ForMember(dest => dest.TaskLabelIds, opt => opt.MapFrom(src => src.TaskLabels.Select(label => label.Id).ToList()))
            .ReverseMap();

        CreateMap<TaskLabel, TaskLabelUpdate>();

    }
}
