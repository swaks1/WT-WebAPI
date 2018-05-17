using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WT_WebAPI.Entities.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    NotificationActivated = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BodyStatistics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    WTUserID = table.Column<int>(nullable: true),
                    Week = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyStatistics", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BodyStatistics_Users_WTUserID",
                        column: x => x.WTUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    IsEditable = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    WTUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Exercises_Users_WTUserID",
                        column: x => x.WTUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPrograms",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    IsActivated = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    WTUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPrograms", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkoutPrograms_Users_WTUserID",
                        column: x => x.WTUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutRoutines",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PlannedDate = table.Column<DateTime>(nullable: true),
                    WTUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutRoutines", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkoutRoutines_Users_WTUserID",
                        column: x => x.WTUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSessions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: true),
                    WTUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSessions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkoutSessions_Users_WTUserID",
                        column: x => x.WTUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BodyStatAttribute",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttributeName = table.Column<string>(nullable: true),
                    AttributeValue = table.Column<string>(nullable: true),
                    BodyStatisticID = table.Column<int>(nullable: true),
                    IsDeletable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyStatAttribute", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BodyStatAttribute_BodyStatistics_BodyStatisticID",
                        column: x => x.BodyStatisticID,
                        principalTable: "BodyStatistics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgressImage",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BodyStatisticID = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressImage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProgressImage_BodyStatistics_BodyStatisticID",
                        column: x => x.BodyStatisticID,
                        principalTable: "BodyStatistics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseAttribute",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttributeName = table.Column<string>(nullable: true),
                    AttributeValue = table.Column<string>(nullable: true),
                    ExerciseID = table.Column<int>(nullable: true),
                    IsDeletable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseAttribute", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExerciseAttribute_Exercises_ExerciseID",
                        column: x => x.ExerciseID,
                        principalTable: "Exercises",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseRoutineEntry",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExerciseID = table.Column<int>(nullable: true),
                    IsReserved = table.Column<bool>(nullable: false),
                    WorkoutRoutineID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseRoutineEntry", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExerciseRoutineEntry_Exercises_ExerciseID",
                        column: x => x.ExerciseID,
                        principalTable: "Exercises",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseRoutineEntry_WorkoutRoutines_WorkoutRoutineID",
                        column: x => x.WorkoutRoutineID,
                        principalTable: "WorkoutRoutines",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoutineProgramEntry",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WorkoutProgramID = table.Column<int>(nullable: true),
                    WorkoutRoutineID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineProgramEntry", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoutineProgramEntry_WorkoutPrograms_WorkoutProgramID",
                        column: x => x.WorkoutProgramID,
                        principalTable: "WorkoutPrograms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoutineProgramEntry_WorkoutRoutines_WorkoutRoutineID",
                        column: x => x.WorkoutRoutineID,
                        principalTable: "WorkoutRoutines",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseSessionEntry",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExerciseID = table.Column<int>(nullable: true),
                    WorkoutSessionID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseSessionEntry", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExerciseSessionEntry_Exercises_ExerciseID",
                        column: x => x.ExerciseID,
                        principalTable: "Exercises",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseSessionEntry_WorkoutSessions_WorkoutSessionID",
                        column: x => x.WorkoutSessionID,
                        principalTable: "WorkoutSessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BodyStatAttribute_BodyStatisticID",
                table: "BodyStatAttribute",
                column: "BodyStatisticID");

            migrationBuilder.CreateIndex(
                name: "IX_BodyStatistics_WTUserID",
                table: "BodyStatistics",
                column: "WTUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseAttribute_ExerciseID",
                table: "ExerciseAttribute",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseRoutineEntry_ExerciseID",
                table: "ExerciseRoutineEntry",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseRoutineEntry_WorkoutRoutineID",
                table: "ExerciseRoutineEntry",
                column: "WorkoutRoutineID");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_WTUserID",
                table: "Exercises",
                column: "WTUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSessionEntry_ExerciseID",
                table: "ExerciseSessionEntry",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSessionEntry_WorkoutSessionID",
                table: "ExerciseSessionEntry",
                column: "WorkoutSessionID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressImage_BodyStatisticID",
                table: "ProgressImage",
                column: "BodyStatisticID");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineProgramEntry_WorkoutProgramID",
                table: "RoutineProgramEntry",
                column: "WorkoutProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineProgramEntry_WorkoutRoutineID",
                table: "RoutineProgramEntry",
                column: "WorkoutRoutineID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPrograms_WTUserID",
                table: "WorkoutPrograms",
                column: "WTUserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutRoutines_WTUserID",
                table: "WorkoutRoutines",
                column: "WTUserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_WTUserID",
                table: "WorkoutSessions",
                column: "WTUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BodyStatAttribute");

            migrationBuilder.DropTable(
                name: "ExerciseAttribute");

            migrationBuilder.DropTable(
                name: "ExerciseRoutineEntry");

            migrationBuilder.DropTable(
                name: "ExerciseSessionEntry");

            migrationBuilder.DropTable(
                name: "ProgressImage");

            migrationBuilder.DropTable(
                name: "RoutineProgramEntry");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "WorkoutSessions");

            migrationBuilder.DropTable(
                name: "BodyStatistics");

            migrationBuilder.DropTable(
                name: "WorkoutPrograms");

            migrationBuilder.DropTable(
                name: "WorkoutRoutines");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
