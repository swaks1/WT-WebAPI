using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities
{
    public class Exercise
    {
        public int ExerciseID { get; set; }
        public string Name { get; set; }

        public int? WTUserID { get; set; }
        public WTUser User { get; set; }
    }
}
