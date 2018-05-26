using System;
using System.Collections.Generic;
using System.Text;
using WT_WebAPI.Entities.DTO.WorkoutAssets;

namespace WT_WebAPI.Entities.DTO
{
    public class WorkoutSessionRequest
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? CurrentDate { get; set; }

        public List<WorkoutRoutineDTO> Routines { get; set; }

        public List<ExerciseDTO> Exercises { get; set; }

        public List<ConcreteExerciseDTO> ConcreteExercises { get; set; }
    }
}
