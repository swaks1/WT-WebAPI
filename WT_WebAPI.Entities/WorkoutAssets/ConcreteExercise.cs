﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.WorkoutAssets
{
    public class ConcreteExercise
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public int? RoutineId { get; set; }
        public string RoutineName { get; set; }

        public int? ProgramId { get; set; }
        public string ProgramName { get; set; }

        //Relationships (Navigational properties)
        public int? WTUserID { get; set; }
        public WTUser User { get; set; }

        public int? WorkoutSessionID { get; set; }
        public WorkoutSession WorkoutSession { get; set; }

        public ICollection<ConcreteExerciseAttribute> Attributes { get; set; }
    }
}
