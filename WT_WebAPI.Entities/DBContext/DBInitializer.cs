﻿using System;
using System.Collections.Generic;
using System.Linq;
using WT_WebAPI.Entities.WorkoutAssets;

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
                new Exercise{Name = "Riste Execise 1", WTUserID = 1,Category = Category.ARMS,Description ="Riste Execise Desc 1",IsEditable = true},
                new Exercise{Name = "Riste Execise 2", WTUserID = 1,Category = Category.CHEST,Description ="Riste Execise Desc 2",IsEditable = true},
                new Exercise{Name = "Monkas Execise 1", WTUserID = 2,Category = Category.LEGS,Description ="Monkas Execise Desc 1",IsEditable = true},
                new Exercise{Name = "Monkas Execise 2", WTUserID = 2,Category = Category.OTHER,Description ="Monkas Execise Desc 2",IsEditable = true},
            };
            context.Exercises.AddRange(exercises);
            context.SaveChanges();


            var routines = new WorkoutRoutine[]
            {
                new WorkoutRoutine{Name = "Riste Workout 1",Description = "Riste Routine Desc 1", WTUserID = 1},
                new WorkoutRoutine{Name = "Riste Workout 2",Description = "Riste Routine Desc 2", WTUserID = 1},
                new WorkoutRoutine{Name = "Monkas Workout 1",Description = "Monkas Routine Desc 1", WTUserID = 2},
                new WorkoutRoutine{Name = "Monkas Workout 1",Description = "Monkas Routine Desc 2" ,WTUserID =2 }
            };
            context.WorkoutRoutines.AddRange(routines);
            context.SaveChanges();


            var programs = new WorkoutProgram[]
            {
                new WorkoutProgram{Name = "Riste Program 1",Description = "Riste Program Desc 1", WTUserID = 1},
                new WorkoutProgram{Name = "Riste Program 2",Description = "Riste Program Desc 2", WTUserID = 1},
                new WorkoutProgram{Name = "Monkas Program 1",Description = "Monkas Program Desc 1", WTUserID = 2},
                new WorkoutProgram{Name = "Monkas Program 1",Description = "Monkas Program Desc 2", WTUserID = 2},
            };
            context.WorkoutPrograms.AddRange(programs);
            context.SaveChanges();
            

            var customAttribute1 = new ExerciseAttribute { AttributeName = "Riste Cust Attr 1", AttributeValue = "value1", ExerciseID = 1, IsDeletable = true };
            var customAttribute2 = new ExerciseAttribute { AttributeName = "Riste Cust Attr 2", AttributeValue = "value2", ExerciseID = 2, IsDeletable = true };
            var customAttribute3 = new ExerciseAttribute { AttributeName = "Monkas Cust Attr 1", AttributeValue = "value1", ExerciseID = 3, IsDeletable = true };
            var customAttribute4 = new ExerciseAttribute { AttributeName = "Monkas Cust Attr 2", AttributeValue = "value2", ExerciseID = 4, IsDeletable = true };
            var exercise = context.Exercises.SingleOrDefault(e => e.ID == 1);
            exercise.Attributes = new List<ExerciseAttribute>();
            exercise.Attributes.Add(customAttribute1);

            exercise = context.Exercises.SingleOrDefault(e => e.ID == 2);
            exercise.Attributes = new List<ExerciseAttribute>();
            exercise.Attributes.Add(customAttribute2);

            exercise = context.Exercises.SingleOrDefault(e => e.ID == 3);
            exercise.Attributes = new List<ExerciseAttribute>();
            exercise.Attributes.Add(customAttribute3);

            exercise = context.Exercises.SingleOrDefault(e => e.ID == 4);
            exercise.Attributes = new List<ExerciseAttribute>();
            exercise.Attributes.Add(customAttribute4);
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



            var routineProgramEntry1 = new RoutineProgramEntry { WorkoutRoutineID = 1, WorkoutProgramID = 1 };
            var routineProgramEntry2 = new RoutineProgramEntry { WorkoutRoutineID = 2, WorkoutProgramID = 1 };
            var routineProgramEntry3 = new RoutineProgramEntry { WorkoutRoutineID = 1, WorkoutProgramID = 2 };
            var program = context.WorkoutPrograms.SingleOrDefault(u => u.ID == 1);
            program.RoutineProgramEntries = new List<RoutineProgramEntry>();
            program.RoutineProgramEntries.Add(routineProgramEntry1);
            program.RoutineProgramEntries.Add(routineProgramEntry2);

            program = context.WorkoutPrograms.SingleOrDefault(u => u.ID == 2);
            program.RoutineProgramEntries = new List<RoutineProgramEntry>();
            program.RoutineProgramEntries.Add(routineProgramEntry3);

            var routineProgramEntry4 = new RoutineProgramEntry { WorkoutRoutineID = 3, WorkoutProgramID = 3 };
            var routineProgramEntry5 = new RoutineProgramEntry { WorkoutRoutineID = 4, WorkoutProgramID = 3 };
            var routineProgramEntry6 = new RoutineProgramEntry { WorkoutRoutineID = 3, WorkoutProgramID = 4 };
            program = context.WorkoutPrograms.SingleOrDefault(u => u.ID == 3);
            program.RoutineProgramEntries = new List<RoutineProgramEntry>();
            program.RoutineProgramEntries.Add(routineProgramEntry4);
            program.RoutineProgramEntries.Add(routineProgramEntry5);

            program = context.WorkoutPrograms.SingleOrDefault(u => u.ID == 4);
            program.RoutineProgramEntries = new List<RoutineProgramEntry>();
            program.RoutineProgramEntries.Add(routineProgramEntry6);
            context.SaveChanges();
        }
    }
}