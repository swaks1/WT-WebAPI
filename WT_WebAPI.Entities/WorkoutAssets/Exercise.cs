using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WT_WebAPI.Entities.WorkoutAssets
{
    public class Exercise
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsEditable { get; set; }
        [NotMapped]
        public byte[] ImageBytes { get; set; }


        //Relationships (Navigational properties)
        public int? WTUserID { get; set; }
        public WTUser User { get; set; }

        public ICollection<ExerciseAttribute> Attributes { get; set; }

        //here are the Routines that have this exercise
        public ICollection<ExerciseRoutineEntry> ExerciseRoutineEntries { get; set; }

    }
}
