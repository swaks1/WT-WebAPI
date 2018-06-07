using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.WorkoutAssets
{
    public class ExerciseAttribute
    {
        public int ID { get; set; }

        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public bool IsDeletable { get; set; } = true;

        public int? ExerciseID { get; set; }
        public Exercise Exercise { get; set; }
    }
}
