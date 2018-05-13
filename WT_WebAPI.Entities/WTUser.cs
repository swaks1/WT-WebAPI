using System;
using System.Collections.Generic;

namespace WT_WebAPI.Entities
{
    public class WTUser
    {
        public int WTUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Exercise> Exercises
        {
            get; set;
        }
    }
}
