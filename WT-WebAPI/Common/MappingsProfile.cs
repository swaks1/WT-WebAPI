using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT_WebAPI.Entities;
using WT_WebAPI.Entities.DTO;
using WT_WebAPI.Entities.DTO.WorkoutAssets;
using WT_WebAPI.Entities.DTO.WorkoutProgress;
using WT_WebAPI.Entities.WorkoutAssets;
using WT_WebAPI.Entities.WorkoutProgress;

namespace WT_WebAPI.Common
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<WTUser, WTUserDTO>();
            CreateMap<WorkoutRoutine, WorkoutRoutineDTO>().ForMember(dest => dest.Exercises,
                                                                       opts => opts.MapFrom(src => src.ExerciseRoutineEntries.Select(ex => ex.Exercise)));

            CreateMap<WorkoutProgram, WorkoutProgramDTO>().ForMember(dest => dest.WorkoutRoutines,
                                                           opts => opts.MapFrom(src => src.RoutineProgramEntries.Select(ex => ex.WorkoutRoutine)));

            CreateMap<WorkoutSession, WorkoutSessionDTO>().ForMember(dest => dest.Exercises,
                                               opts => opts.MapFrom(src => src.ExerciseSessionEntries.Select(ex => ex.Exercise)));

            CreateMap<Exercise, ExerciseDTO>();//.ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<BodyStatistic, BodyStatisticDTO>();
        }
    }
}
