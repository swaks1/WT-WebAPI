using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.WorkoutAssets
{
    public class WorkoutSession
    {
        public int ID { get; set; }
        public DateTime? Date { get; set; }


        public int? WTUserID { get; set; }
        public WTUser User { get; set; }

        public ICollection<ConcreteExerciseSessionEntry> ConcreteExerciseEntries { get; set; }
       
    }
}
