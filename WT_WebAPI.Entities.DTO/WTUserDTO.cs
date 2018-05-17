using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT_WebAPI.Entities.DTO.WorkoutAssets;
using WT_WebAPI.Entities.DTO.WorkoutProgress;

namespace WT_WebAPI.Entities.DTO
{
    public class WTUserDTO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool NotificationActivated { get; set; }

        //Navigational Properties        
        public ICollection<ExerciseDTO> Exercises { get; set; }
        public ICollection<WorkoutRoutineDTO> WorkoutRoutines { get; set; }
        public ICollection<WorkoutProgramDTO> WorkoutPrograms { get; set; }
        public ICollection<WorkoutSessionDTO> WorkoutSession { get; set; }
        public ICollection<BodyStatisticDTO> BodyStatistics { get; set; }
    }
}
