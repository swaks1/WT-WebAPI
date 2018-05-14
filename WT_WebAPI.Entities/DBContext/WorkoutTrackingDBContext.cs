using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WT_WebAPI.Entities.WorkoutAssets;

namespace WT_WebAPI.Entities.DBContext
{
    public class WorkoutTrackingDBContext : DbContext
    {
        public WorkoutTrackingDBContext(DbContextOptions<WorkoutTrackingDBContext> options) : base(options)
        {
        }

        public DbSet<WTUser> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutRoutine> WorkoutRoutines { get; set; }
        public DbSet<WorkoutProgram> WorkoutPrograms { get; set; }

    }
}
