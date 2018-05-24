using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WT_WebAPI.Entities.WorkoutAssets;

namespace WT_WebAPI.Entities.Extensions
{
    public static class CustomExtensions
    {
        static GregorianCalendar _gc = new GregorianCalendar();


        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        static int GetWeekOfYear(this DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        public static ConcreteExercise GetConcreteExerciseObject(this Exercise exercise)
        {
            ConcreteExercise efs = new ConcreteExercise
            {
                Name = exercise.Name,
                WTUserID = exercise.WTUserID,
                Category = exercise.Category,
                Description = exercise.Description,
                ImagePath = exercise.ImagePath,                
            };

            efs.Attributes = exercise.Attributes.Select(item => new ConcreteExerciseAttribute
            {
                AttributeName = item.AttributeName,
                AttributeValue = item.AttributeValue,
                IsDeletable = true
            }).ToList();

            return efs;
        }
    }

}
