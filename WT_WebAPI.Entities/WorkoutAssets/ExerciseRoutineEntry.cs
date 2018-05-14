using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.WorkoutAssets
{
    //JOIN ENTITY used for decoupling many to many relationship
    public class ExerciseRoutineEntry
    {
        public int ID { get; set; }

        public int? ExerciseID { get; set; }
        public Exercise Exercise { get; set; }

        public int? WorkoutRoutineID { get; set; }
        public WorkoutRoutine WorkoutRoutine { get; set; }
    }
}
