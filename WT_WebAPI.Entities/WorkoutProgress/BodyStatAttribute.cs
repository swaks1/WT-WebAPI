using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.WorkoutProgress
{
    public class BodyStatAttribute
    {
        public int ID { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public bool IsDeletable { get; set; }

        public int? BodyStatisticID { get; set; }
        public BodyStatistic BodyStatistic { get; set; }
    }
}
