using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.DTO.WorkoutProgress
{
    public class BodyStatisticDTO
    {
        public int ID { get; set; }
        public DateTime? DateCreated { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }

        public ICollection<BodyStatAttributeDTO> BodyStatAttributes { get; set; }
        public ICollection<ProgressImageDTO> ProgressImages { get; set; }

        public int? WTUserID { get; set; }
    }
}
