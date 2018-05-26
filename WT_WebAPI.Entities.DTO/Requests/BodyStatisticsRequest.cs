using System;
using System.Collections.Generic;
using System.Text;
using WT_WebAPI.Entities.DTO.WorkoutProgress;

namespace WT_WebAPI.Entities.DTO.Requests
{
    public class BodyStatisticsRequest
    {
        public int ID { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }

        public ICollection<BodyAttributeTemplateDTO> BodyAttributeTemplates { get; set; }
        public ICollection<ProgressImageDTO> ProgressImages { get; set; }

    }
}
