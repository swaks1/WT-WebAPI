﻿using System;
using System.Collections.Generic;
using System.Linq;
using WT_WebAPI.Entities.WorkoutAssets;
using WT_WebAPI.Entities.WorkoutProgress;
using WT_WebAPI.Entities.Extensions;

namespace WT_WebAPI.Entities.DBContext
{
    public static class DBInitializer
    {
        public static void Initialize(WorkoutTrackingDBContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new WTUser[]
            {
                new WTUser{FirstName="Riste",LastName="Poposki",RegisterDate=DateTime.Parse("2017-09-01"),Email = "r@r.com",Password="pass123",Username="RistePoposki"},
                new WTUser{FirstName="Monkas",LastName="MonkaGiga",RegisterDate=DateTime.Now, Email = "m@m.com",Password="pass1234",Username="MonkaOmega"},
            };
            context.Users.AddRange(users);
            context.SaveChanges();


            var exercises = new Exercise[]
            {
                new Exercise{Name = "Riste Execise 1", WTUserID = 1,Category = Category.ARMS,Description ="Riste Execise Desc 1",IsEditable = true, ImagePath="Images/monkas.jpg"},
                new Exercise{Name = "Riste Execise 2", WTUserID = 1,Category = Category.CHEST,Description ="Riste Execise Desc 2",IsEditable = true, ImagePath="Images/monkas.jpg"},
                new Exercise{Name = "Monkas Execise 1", WTUserID = 2,Category = Category.LEGS,Description ="Monkas Execise Desc 1",IsEditable = true, ImagePath="Images/monkas.jpg"},
                new Exercise{Name = "Monkas Execise 2", WTUserID = 2,Category = Category.OTHER,Description ="Monkas Execise Desc 2",IsEditable = true, ImagePath="Images/monkas.jpg"}
            };
            context.Exercises.AddRange(exercises);
            context.SaveChanges();


            var routines = new WorkoutRoutine[]
            {
                new WorkoutRoutine{Name = "Riste Workout 1",Description = "Riste Routine Desc 1", WTUserID = 1, ImagePath = "Images/monkasRoutine.jpg"},
                new WorkoutRoutine{Name = "Riste Workout 2",Description = "Riste Routine Desc 2", WTUserID = 1, ImagePath = "Images/monkasRoutine.jpg"},
                new WorkoutRoutine{Name = "Monkas Workout 1",Description = "Monkas Routine Desc 1", WTUserID = 2,ImagePath = "Images/monkasRoutine.jpg"},
                new WorkoutRoutine{Name = "Monkas Workout 1",Description = "Monkas Routine Desc 2" ,WTUserID = 2 ,ImagePath = "Images/monkasRoutine.jpg"}
            };
            context.WorkoutRoutines.AddRange(routines);
            context.SaveChanges();


            var programs = new WorkoutProgram[]
            {
                new WorkoutProgram{Name = "Riste Program 1",Description = "Riste Program Desc 1", WTUserID = 1, ImagePath = "Images/monkasProgram.png"},
                new WorkoutProgram{Name = "Riste Program 2",Description = "Riste Program Desc 2", WTUserID = 1, ImagePath = "Images/monkasProgram.png"},
                new WorkoutProgram{Name = "Monkas Program 1",Description = "Monkas Program Desc 1", WTUserID = 2, ImagePath = "Images/monkasProgram.png"},
                new WorkoutProgram{Name = "Monkas Program 1",Description = "Monkas Program Desc 2", WTUserID = 2, ImagePath = "Images/monkasProgram.png"},
            };
            context.WorkoutPrograms.AddRange(programs);
            context.SaveChanges();


            var sessions = new WorkoutSession[]
            {
                new WorkoutSession{Date = DateTime.Now, WTUserID = 1},
                new WorkoutSession{Date = DateTime.Now.AddDays(1), WTUserID = 1},
                new WorkoutSession{Date = DateTime.Now, WTUserID = 2},
                new WorkoutSession{Date = DateTime.Now.AddDays(1), WTUserID = 2},
            };
            context.WorkoutSessions.AddRange(sessions);
            context.SaveChanges();


            var bodyStats = new BodyStatistic[]
            {
                new BodyStatistic{DateCreated = DateTime.Now, Year = DateTime.Now.Year, Month = DateTime.Now.Month, Week = DateTime.Now.GetWeekOfMonth(), WTUserID = 1},
                new BodyStatistic{DateCreated = DateTime.Now.AddDays(7), Year = DateTime.Now.AddDays(7).Year,
                                    Month = DateTime.Now.AddDays(7).Month, Week = DateTime.Now.AddDays(7).GetWeekOfMonth(), WTUserID = 1},

                new BodyStatistic{DateCreated = DateTime.Now, Year = DateTime.Now.Year, Month = DateTime.Now.Month, Week = DateTime.Now.GetWeekOfMonth(), WTUserID = 2},
                new BodyStatistic{DateCreated = DateTime.Now.AddDays(7), Year = DateTime.Now.AddDays(7).Year,
                                    Month = DateTime.Now.AddDays(7).Month, Week = DateTime.Now.AddDays(7).GetWeekOfMonth(), WTUserID = 2},

            };
            context.BodyStatistics.AddRange(bodyStats);
            context.SaveChanges();



            var bodyAttributeTemplate = new BodyAttributeTemplate[]
            {
                new BodyAttributeTemplate{AttributeName = "Template Attr 1", AttributeValue = "OK1", WTUserID = 1, IsDeletable = true},
                new BodyAttributeTemplate{AttributeName = "Template Attr 2", AttributeValue = "OK2", WTUserID = 1, IsDeletable = true},
                new BodyAttributeTemplate{AttributeName = "Template Attr 3", AttributeValue = "NO1", WTUserID = 2, IsDeletable = true},
                new BodyAttributeTemplate{AttributeName = "Template Attr 4" , AttributeValue = "NO2", WTUserID = 2, IsDeletable = true}
            };
            context.BodyAttributeTemplates.AddRange(bodyAttributeTemplate);
            context.SaveChanges();




            var exerciseAttribute1 = new ExerciseAttribute { AttributeName = "Sets", AttributeValue = "value1", ExerciseID = 1, IsDeletable = false };
            var exerciseAttribute2 = new ExerciseAttribute { AttributeName = "Repetitions", AttributeValue = "value2", ExerciseID = 2, IsDeletable = false };
            var exerciseAttribute3 = new ExerciseAttribute { AttributeName = "Sets", AttributeValue = "value1", ExerciseID = 3, IsDeletable = false };
            var exerciseAttribute4 = new ExerciseAttribute { AttributeName = "Repetitions", AttributeValue = "value2", ExerciseID = 4, IsDeletable = false };

            var exercise = context.Exercises.SingleOrDefault(e => e.ID == 1);
            exercise.Attributes = new List<ExerciseAttribute>();
            exercise.Attributes.Add(exerciseAttribute1);

            exercise = context.Exercises.SingleOrDefault(e => e.ID == 2);
            exercise.Attributes = new List<ExerciseAttribute>();
            exercise.Attributes.Add(exerciseAttribute2);

            exercise = context.Exercises.SingleOrDefault(e => e.ID == 3);
            exercise.Attributes = new List<ExerciseAttribute>();
            exercise.Attributes.Add(exerciseAttribute3);

            exercise = context.Exercises.SingleOrDefault(e => e.ID == 4);
            exercise.Attributes = new List<ExerciseAttribute>();
            exercise.Attributes.Add(exerciseAttribute4);
            context.SaveChanges();




            var bodyStatAttribute1 = new BodyStatAttribute { AttributeName = "Riste Height", AttributeValue = "173", BodyStatisticID = 1, IsDeletable = true };
            var bodyStatAttribute2 = new BodyStatAttribute { AttributeName = "Riste Weight", AttributeValue = "71", BodyStatisticID = 2, IsDeletable = true };
            var bodyStatAttribute3 = new BodyStatAttribute { AttributeName = "Monkas Height", AttributeValue = "195", BodyStatisticID = 3, IsDeletable = true };
            var bodyStatAttribute4 = new BodyStatAttribute { AttributeName = "Monkas Weight", AttributeValue = "91", BodyStatisticID = 4, IsDeletable = true };

            var bodyStat = context.BodyStatistics.SingleOrDefault(e => e.ID == 1);
            bodyStat.BodyStatAttributes = new List<BodyStatAttribute>();
            bodyStat.BodyStatAttributes.Add(bodyStatAttribute1);

            bodyStat = context.BodyStatistics.SingleOrDefault(e => e.ID == 2);
            bodyStat.BodyStatAttributes = new List<BodyStatAttribute>();
            bodyStat.BodyStatAttributes.Add(bodyStatAttribute2);

            bodyStat = context.BodyStatistics.SingleOrDefault(e => e.ID == 3);
            bodyStat.BodyStatAttributes = new List<BodyStatAttribute>();
            bodyStat.BodyStatAttributes.Add(bodyStatAttribute3);

            bodyStat = context.BodyStatistics.SingleOrDefault(e => e.ID == 4);
            bodyStat.BodyStatAttributes = new List<BodyStatAttribute>();
            bodyStat.BodyStatAttributes.Add(bodyStatAttribute4);
            context.SaveChanges();





            var progressImage1 = new ProgressImage { DateCreated = DateTime.Now, Url = "url1", BodyStatisticID = 1 };
            var progressImage2 = new ProgressImage { DateCreated = DateTime.Now.AddDays(1), Url = "url2", BodyStatisticID = 2 };
            var progressImage3 = new ProgressImage { DateCreated = DateTime.Now.AddDays(2), Url = "url3", BodyStatisticID = 3 };
            var progressImage4 = new ProgressImage { DateCreated = DateTime.Now.AddDays(3), Url = "url4", BodyStatisticID = 4 };

            bodyStat = context.BodyStatistics.SingleOrDefault(e => e.ID == 1);
            bodyStat.ProgressImages = new List<ProgressImage>();
            bodyStat.ProgressImages.Add(progressImage1);

            bodyStat = context.BodyStatistics.SingleOrDefault(e => e.ID == 2);
            bodyStat.ProgressImages = new List<ProgressImage>();
            bodyStat.ProgressImages.Add(progressImage2);

            bodyStat = context.BodyStatistics.SingleOrDefault(e => e.ID == 3);
            bodyStat.ProgressImages = new List<ProgressImage>();
            bodyStat.ProgressImages.Add(progressImage3);

            bodyStat = context.BodyStatistics.SingleOrDefault(e => e.ID == 4);
            bodyStat.ProgressImages = new List<ProgressImage>();
            bodyStat.ProgressImages.Add(progressImage4);
            context.SaveChanges();




            var exerciseRoutineEntry1 = new ExerciseRoutineEntry { ExerciseID = 1, WorkoutRoutineID = 1 };
            var exerciseRoutineEntry2 = new ExerciseRoutineEntry { ExerciseID = 2, WorkoutRoutineID = 1 };
            var exerciseRoutineEntry3 = new ExerciseRoutineEntry { ExerciseID = 1, WorkoutRoutineID = 2 };

            var routine = context.WorkoutRoutines.SingleOrDefault(u => u.ID == 1);
            routine.ExerciseRoutineEntries = new List<ExerciseRoutineEntry>();
            routine.ExerciseRoutineEntries.Add(exerciseRoutineEntry1);
            routine.ExerciseRoutineEntries.Add(exerciseRoutineEntry2);

            routine = context.WorkoutRoutines.SingleOrDefault(u => u.ID == 2);
            routine.ExerciseRoutineEntries = new List<ExerciseRoutineEntry>();
            routine.ExerciseRoutineEntries.Add(exerciseRoutineEntry3);

            var exerciseRoutineEntry4 = new ExerciseRoutineEntry { ExerciseID = 3, WorkoutRoutineID = 3 };
            var exerciseRoutineEntry5 = new ExerciseRoutineEntry { ExerciseID = 4, WorkoutRoutineID = 3 };
            var exerciseRoutineEntry6 = new ExerciseRoutineEntry { ExerciseID = 3, WorkoutRoutineID = 4 };

            routine = context.WorkoutRoutines.SingleOrDefault(u => u.ID == 3);
            routine.ExerciseRoutineEntries = new List<ExerciseRoutineEntry>();
            routine.ExerciseRoutineEntries.Add(exerciseRoutineEntry4);
            routine.ExerciseRoutineEntries.Add(exerciseRoutineEntry5);

            routine = context.WorkoutRoutines.SingleOrDefault(u => u.ID == 4);
            routine.ExerciseRoutineEntries = new List<ExerciseRoutineEntry>();
            routine.ExerciseRoutineEntries.Add(exerciseRoutineEntry6);
            context.SaveChanges();




            var routineProgramEntry1 = new RoutineProgramEntry { WorkoutRoutineID = 1, WorkoutProgramID = 1, PlannedDates = "2018-05-26;2018-06-01" };
            var routineProgramEntry2 = new RoutineProgramEntry { WorkoutRoutineID = 2, WorkoutProgramID = 1, PlannedDates = "2018-05-29" };
            var routineProgramEntry3 = new RoutineProgramEntry { WorkoutRoutineID = 1, WorkoutProgramID = 2, PlannedDates = "2018-07-26;2018-07-27" };

            var program = context.WorkoutPrograms.SingleOrDefault(u => u.ID == 1);
            program.RoutineProgramEntries = new List<RoutineProgramEntry>();
            program.RoutineProgramEntries.Add(routineProgramEntry1);
            program.RoutineProgramEntries.Add(routineProgramEntry2);

            program = context.WorkoutPrograms.SingleOrDefault(u => u.ID == 2);
            program.RoutineProgramEntries = new List<RoutineProgramEntry>();
            program.RoutineProgramEntries.Add(routineProgramEntry3);

            var routineProgramEntry4 = new RoutineProgramEntry { WorkoutRoutineID = 3, WorkoutProgramID = 3, PlannedDates = "2018-01-26;2018-01-27" };
            var routineProgramEntry5 = new RoutineProgramEntry { WorkoutRoutineID = 4, WorkoutProgramID = 3, PlannedDates = "2018-02-26" };
            var routineProgramEntry6 = new RoutineProgramEntry { WorkoutRoutineID = 3, WorkoutProgramID = 4, PlannedDates = "2018-03-26;2018-03-27" };

            program = context.WorkoutPrograms.SingleOrDefault(u => u.ID == 3);
            program.RoutineProgramEntries = new List<RoutineProgramEntry>();
            program.RoutineProgramEntries.Add(routineProgramEntry4);
            program.RoutineProgramEntries.Add(routineProgramEntry5);

            program = context.WorkoutPrograms.SingleOrDefault(u => u.ID == 4);
            program.RoutineProgramEntries = new List<RoutineProgramEntry>();
            program.RoutineProgramEntries.Add(routineProgramEntry6);
            context.SaveChanges();



            var ConcreteExercisesList = exercises.ToList().Select(item => item.GetConcreteExerciseObject()).ToList();
            for (int i = 0; i < ConcreteExercisesList.Count(); i++)
            {
                var item = ConcreteExercisesList[i];
                item.WorkoutSessionID = i + 1;
                context.ConcreteExercises.Add(item);
            }
            context.SaveChanges();

        }
    }
}