using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.WorkoutProgress
{
    public class ProgressImage
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public DateTime? DateCreated { get; set; }

        public int? BodyStatisticID { get; set; }
        public BodyStatistic BodyStatistic { get; set; }
    }
}
