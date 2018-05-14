using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.WorkoutAssets
{
    //JOIN ENTITY used for decoupling many to many relationship
    public class RoutineProgramEntry
    {
        public int ID { get; set; }

        public int? WorkoutRoutineID { get; set; }
        public WorkoutRoutine WorkoutRoutine { get; set; }

        public int? WorkoutProgramID { get; set; }
        public WorkoutProgram WorkoutProgram { get; set; }
    }
}
