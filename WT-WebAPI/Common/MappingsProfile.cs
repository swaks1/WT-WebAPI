﻿using AutoMapper;
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
            CreateMap<WTUserDTO, WTUser>();

            CreateMap<WorkoutRoutine, WorkoutRoutineDTO>().ForMember(dest => dest.Exercises,
                                                            opts => opts.MapFrom(src => src.ExerciseRoutineEntries.Select(ex => ex.Exercise)))
                                                          .ForMember(dest => dest.ProgramsIds,
                                                            opts => opts.MapFrom(src => src.RoutineProgramEntries.Select(prog => prog.WorkoutProgramID)));

            CreateMap<WorkoutRoutineDTO, WorkoutRoutine>().ForMember(dest => dest.ExerciseRoutineEntries,
                                                              opts => opts.MapFrom(src => src.Exercises.Select(ex => new ExerciseRoutineEntry { ExerciseID = ex.ID })))
                                                           .ForMember(dest => dest.RoutineProgramEntries,
                                                              opts => opts.MapFrom(src => src.ProgramsIds.Select(progId => new RoutineProgramEntry { WorkoutProgramID = progId })));




            CreateMap<WorkoutProgram, WorkoutProgramDTO>().ForMember(dest => dest.WorkoutRoutines,
                                                opts => opts.MapFrom(src => src.RoutineProgramEntries.Select(ex => ex.WorkoutRoutine)));

            CreateMap<WorkoutProgramDTO, WorkoutProgram>().ForMember(dest => dest.RoutineProgramEntries,
                                                opts => opts.MapFrom(src => src.WorkoutRoutines.Select(rout => new RoutineProgramEntry { WorkoutRoutineID = rout.ID })));



            CreateMap<WorkoutSession, WorkoutSessionDTO>().ForMember(dest => dest.Exercises,
                                               opts => opts.MapFrom(src => src.ExerciseSessionEntries.Select(ex => ex.Exercise)));

            CreateMap<WorkoutSessionDTO, WorkoutSession>().ForMember(dest => dest.ExerciseSessionEntries,
                                               opts => opts.MapFrom(src => src.Exercises.Select(ex => new ExerciseSessionEntry { ExerciseID = ex.ID })));


            CreateMap<Exercise, ExerciseDTO>();//.ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<ExerciseDTO, Exercise>();


            CreateMap<ExerciseAttribute, ExerciseAttributeDTO>();
            CreateMap<ExerciseAttributeDTO, ExerciseAttribute>();


            CreateMap<BodyStatistic, BodyStatisticDTO>();
        }
    }
}
