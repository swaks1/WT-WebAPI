﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WT_WebAPI.Entities.DBContext;
using WT_WebAPI.Entities.WorkoutAssets;

namespace WT_WebAPI.Entities.Migrations
{
    [DbContext(typeof(WorkoutTrackingDBContext))]
    [Migration("20180524130530_concreteExercise")]
    partial class concreteExercise
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.ConcreteExercise", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Category");

                    b.Property<string>("Description");

                    b.Property<string>("ImagePath");

                    b.Property<string>("Name");

                    b.Property<int?>("ProgramId");

                    b.Property<string>("ProgramName");

                    b.Property<int?>("SessionId");

                    b.Property<string>("SesssionName");

                    b.Property<int?>("WTUserID");

                    b.HasKey("ID");

                    b.HasIndex("WTUserID");

                    b.ToTable("ConcreteExercises");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.ConcreteExerciseAttribute", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AttributeName");

                    b.Property<string>("AttributeValue");

                    b.Property<int?>("ConcreteExerciseID");

                    b.Property<bool>("IsDeletable");

                    b.HasKey("ID");

                    b.HasIndex("ConcreteExerciseID");

                    b.ToTable("ConcreteExerciseAttribute");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.ConcreteExerciseSessionEntry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ConcreteExerciseID");

                    b.Property<int?>("WorkoutSessionID");

                    b.HasKey("ID");

                    b.HasIndex("ConcreteExerciseID");

                    b.HasIndex("WorkoutSessionID");

                    b.ToTable("ConcreteExerciseSessionEntry");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.Exercise", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Category");

                    b.Property<string>("Description");

                    b.Property<string>("ImagePath");

                    b.Property<bool>("IsEditable");

                    b.Property<string>("Name");

                    b.Property<int?>("WTUserID");

                    b.HasKey("ID");

                    b.HasIndex("WTUserID");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.ExerciseAttribute", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AttributeName");

                    b.Property<string>("AttributeValue");

                    b.Property<int?>("ExerciseID");

                    b.Property<bool>("IsDeletable");

                    b.HasKey("ID");

                    b.HasIndex("ExerciseID");

                    b.ToTable("ExerciseAttribute");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.ExerciseRoutineEntry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ExerciseID");

                    b.Property<bool>("IsReserved");

                    b.Property<int?>("WorkoutRoutineID");

                    b.HasKey("ID");

                    b.HasIndex("ExerciseID");

                    b.HasIndex("WorkoutRoutineID");

                    b.ToTable("ExerciseRoutineEntry");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.RoutineProgramEntry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("WorkoutProgramID");

                    b.Property<int?>("WorkoutRoutineID");

                    b.HasKey("ID");

                    b.HasIndex("WorkoutProgramID");

                    b.HasIndex("WorkoutRoutineID");

                    b.ToTable("RoutineProgramEntry");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.WorkoutProgram", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("ImagePath");

                    b.Property<bool>("IsActivated");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("StartDate");

                    b.Property<int?>("WTUserID");

                    b.HasKey("ID");

                    b.HasIndex("WTUserID");

                    b.ToTable("WorkoutPrograms");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.WorkoutRoutine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("ImagePath");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("PlannedDate");

                    b.Property<int?>("WTUserID");

                    b.HasKey("ID");

                    b.HasIndex("WTUserID");

                    b.ToTable("WorkoutRoutines");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.WorkoutSession", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Date");

                    b.Property<int?>("WTUserID");

                    b.HasKey("ID");

                    b.HasIndex("WTUserID");

                    b.ToTable("WorkoutSessions");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutProgress.BodyStatAttribute", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AttributeName");

                    b.Property<string>("AttributeValue");

                    b.Property<int?>("BodyStatisticID");

                    b.Property<bool>("IsDeletable");

                    b.HasKey("ID");

                    b.HasIndex("BodyStatisticID");

                    b.ToTable("BodyStatAttribute");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutProgress.BodyStatistic", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateCreated");

                    b.Property<int>("Month");

                    b.Property<int?>("WTUserID");

                    b.Property<int>("Week");

                    b.Property<int>("Year");

                    b.HasKey("ID");

                    b.HasIndex("WTUserID");

                    b.ToTable("BodyStatistics");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutProgress.ProgressImage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BodyStatisticID");

                    b.Property<DateTime?>("DateCreated");

                    b.Property<string>("Url");

                    b.HasKey("ID");

                    b.HasIndex("BodyStatisticID");

                    b.ToTable("ProgressImage");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WTUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("NotificationActivated");

                    b.Property<string>("Password");

                    b.Property<DateTime>("RegisterDate");

                    b.Property<string>("Username");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.ConcreteExercise", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WTUser", "User")
                        .WithMany()
                        .HasForeignKey("WTUserID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.ConcreteExerciseAttribute", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WorkoutAssets.ConcreteExercise", "ConcreteExercise")
                        .WithMany("Attributes")
                        .HasForeignKey("ConcreteExerciseID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.ConcreteExerciseSessionEntry", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WorkoutAssets.ConcreteExercise", "ConcreteExercise")
                        .WithMany()
                        .HasForeignKey("ConcreteExerciseID");

                    b.HasOne("WT_WebAPI.Entities.WorkoutAssets.WorkoutSession", "WorkoutSession")
                        .WithMany("ConcreteExerciseEntries")
                        .HasForeignKey("WorkoutSessionID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.Exercise", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WTUser", "User")
                        .WithMany("Exercises")
                        .HasForeignKey("WTUserID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.ExerciseAttribute", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WorkoutAssets.Exercise", "Exercise")
                        .WithMany("Attributes")
                        .HasForeignKey("ExerciseID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.ExerciseRoutineEntry", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WorkoutAssets.Exercise", "Exercise")
                        .WithMany("ExerciseRoutineEntries")
                        .HasForeignKey("ExerciseID");

                    b.HasOne("WT_WebAPI.Entities.WorkoutAssets.WorkoutRoutine", "WorkoutRoutine")
                        .WithMany("ExerciseRoutineEntries")
                        .HasForeignKey("WorkoutRoutineID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.RoutineProgramEntry", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WorkoutAssets.WorkoutProgram", "WorkoutProgram")
                        .WithMany("RoutineProgramEntries")
                        .HasForeignKey("WorkoutProgramID");

                    b.HasOne("WT_WebAPI.Entities.WorkoutAssets.WorkoutRoutine", "WorkoutRoutine")
                        .WithMany("RoutineProgramEntries")
                        .HasForeignKey("WorkoutRoutineID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.WorkoutProgram", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WTUser", "WTUser")
                        .WithMany("WorkoutPrograms")
                        .HasForeignKey("WTUserID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.WorkoutRoutine", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WTUser", "WTUser")
                        .WithMany("WorkoutRoutines")
                        .HasForeignKey("WTUserID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutAssets.WorkoutSession", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WTUser", "User")
                        .WithMany("WorkoutSession")
                        .HasForeignKey("WTUserID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutProgress.BodyStatAttribute", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WorkoutProgress.BodyStatistic", "BodyStatistic")
                        .WithMany("BodyStatAttributes")
                        .HasForeignKey("BodyStatisticID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutProgress.BodyStatistic", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WTUser", "User")
                        .WithMany("BodyStatistics")
                        .HasForeignKey("WTUserID");
                });

            modelBuilder.Entity("WT_WebAPI.Entities.WorkoutProgress.ProgressImage", b =>
                {
                    b.HasOne("WT_WebAPI.Entities.WorkoutProgress.BodyStatistic", "BodyStatistic")
                        .WithMany("ProgressImages")
                        .HasForeignKey("BodyStatisticID");
                });
#pragma warning restore 612, 618
        }
    }
}
