using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebAPI.Entities.DBContext
{
    public class WorkoutTrackingDBContext : DbContext
    {
        public WorkoutTrackingDBContext(DbContextOptions<WorkoutTrackingDBContext> options) : base(options)
        {
        }

        public DbSet<WTUser> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        
    }
}
