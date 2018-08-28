using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WT_WebAPI.Entities.WorkoutProgress
{
    public class BodyStatistic
    {
        public int ID { get; set; }
        public DateTime? DateCreated { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }

        public ICollection<BodyStatAttribute> BodyStatAttributes { get; set; } = new List<BodyStatAttribute>();
        public ICollection<ProgressImage> ProgressImages { get; set; } = new List<ProgressImage>();
        public string ImagePath { get; set; }
        [NotMapped]
        public byte[] ImageBytes { get; set; }

        public int? WTUserID { get; set; }
        public WTUser User { get; set; }
    }
}
