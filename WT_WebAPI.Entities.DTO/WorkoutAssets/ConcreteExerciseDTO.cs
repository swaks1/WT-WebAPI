using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT_WebAPI.Entities;

namespace WT_WebAPI.Entities.DTO.WorkoutAssets
{
    public class ConcreteExerciseDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public int? SessionId { get; set; }
        public string SesssionName { get; set; }

        public int? ProgramId { get; set; }
        public string ProgramName { get; set; }

        //Relationships (Navigational properties)
        public int? WTUserID { get; set; }

        public ICollection<ConcreteExerciseAttributeDTO> Attributes { get; set; }

    }
}
