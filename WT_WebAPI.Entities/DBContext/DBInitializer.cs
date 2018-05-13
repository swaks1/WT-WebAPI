using System;
using System.Linq;


namespace WT_WebAPI.Entities.DBContext
{
    public static class DBInitializer
    {
        public static void Initialize(WorkoutTrackingDBContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new WTUser[]
            {
            new WTUser{FirstName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
            new WTUser{FirstName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
            };
            foreach (WTUser s in users)
            {
                context.Users.Add(s);
            }
            context.SaveChanges();

            var exercises = new Exercise[]
            {
            new Exercise{Name = "Test Execise 1", WTUserID = context.Users.FirstOrDefault().WTUserID},
            new Exercise{Name = "Test Execise 2", WTUserID = context.Users.LastOrDefault().WTUserID}
            };
            foreach (Exercise c in exercises)
            {
                context.Exercises.Add(c);
            }
            context.SaveChanges();
        }
    }
}