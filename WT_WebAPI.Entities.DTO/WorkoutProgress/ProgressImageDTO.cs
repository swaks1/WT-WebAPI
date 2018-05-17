using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.DTO.WorkoutProgress
{
    public class ProgressImageDTO
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public DateTime? DateCreated { get; set; }

        public int? BodyStatisticID { get; set; }        
    }
}
