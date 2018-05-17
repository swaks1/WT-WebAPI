using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT_WebAPI.Entities;

namespace WT_WebAPI.Entities.DTO.WorkoutAssets
{
    public class ExerciseDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsEditable { get; set; }


        public int? WTUserID { get; set; }

        public ICollection<ExerciseAttributeDTO> Attributes { get; set; }

    }
}
