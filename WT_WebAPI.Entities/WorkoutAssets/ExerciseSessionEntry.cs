using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.WorkoutAssets
{
    //JOIN ENTITY used for decoupling many to many relationship
    public class ExerciseSessionEntry
    {
        public int ID { get; set; }

        public int? ExerciseID { get; set; }
        public Exercise Exercise { get; set; }

        public int? WorkoutSessionID { get; set; }
        public WorkoutSession WorkoutSession { get; set; }
    }
}
