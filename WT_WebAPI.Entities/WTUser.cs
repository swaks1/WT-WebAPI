﻿using System;
using System.Collections.Generic;
using WT_WebAPI.Entities.WorkoutAssets;
using WT_WebAPI.Entities.WorkoutProgress;

namespace WT_WebAPI.Entities
{
    public class WTUser
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool NotificationActivated { get; set; }
        public bool Active { get; set; }        

        //Navigational Properties
        public ICollection<Exercise> Exercises{ get; set; }
        public ICollection<WorkoutRoutine> WorkoutRoutines { get; set; }
        public ICollection<WorkoutProgram> WorkoutPrograms { get; set; }
        public ICollection<WorkoutSession> WorkoutSession { get; set; }
        public ICollection<BodyStatistic> BodyStatistics { get; set; }
        public ICollection<BodyAttributeTemplate> BodyAttributeTemplates { get; set; }

    }
}
