using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.DTO.WorkoutAssets
{
    public class WorkoutSessionDTO
    {
        public int ID { get; set; }
        public DateTime? Date { get; set; }


        public int? WTUserID { get; set; }

        public ICollection<ConcreteExerciseDTO> ConcreteExercises { get; set; }
    }
}
