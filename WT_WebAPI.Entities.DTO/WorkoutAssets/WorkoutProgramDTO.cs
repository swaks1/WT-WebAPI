using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.DTO.WorkoutAssets
{
    public class WorkoutProgramDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActivated { get; set; }

        public int? WTUserID { get; set; }

        public ICollection<WorkoutRoutineDTO> WorkoutRoutines { get; set; }
    }
}
