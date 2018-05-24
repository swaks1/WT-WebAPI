using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WT_WebAPI.Entities.Migrations
{
    public partial class concreteExercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseSessionEntry");

            migrationBuilder.CreateTable(
                name: "ConcreteExercises",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ProgramId = table.Column<int>(nullable: true),
                    ProgramName = table.Column<string>(nullable: true),
                    SessionId = table.Column<int>(nullable: true),
                    SesssionName = table.Column<string>(nullable: true),
                    WTUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcreteExercises", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConcreteExercises_Users_WTUserID",
                        column: x => x.WTUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConcreteExerciseAttribute",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttributeName = table.Column<string>(nullable: true),
                    AttributeValue = table.Column<string>(nullable: true),
                    ConcreteExerciseID = table.Column<int>(nullable: true),
                    IsDeletable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcreteExerciseAttribute", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConcreteExerciseAttribute_ConcreteExercises_ConcreteExerciseID",
                        column: x => x.ConcreteExerciseID,
                        principalTable: "ConcreteExercises",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConcreteExerciseSessionEntry",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcreteExerciseID = table.Column<int>(nullable: true),
                    WorkoutSessionID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcreteExerciseSessionEntry", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConcreteExerciseSessionEntry_ConcreteExercises_ConcreteExerciseID",
                        column: x => x.ConcreteExerciseID,
                        principalTable: "ConcreteExercises",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConcreteExerciseSessionEntry_WorkoutSessions_WorkoutSessionID",
                        column: x => x.WorkoutSessionID,
                        principalTable: "WorkoutSessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteExerciseAttribute_ConcreteExerciseID",
                table: "ConcreteExerciseAttribute",
                column: "ConcreteExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteExercises_WTUserID",
                table: "ConcreteExercises",
                column: "WTUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteExerciseSessionEntry_ConcreteExerciseID",
                table: "ConcreteExerciseSessionEntry",
                column: "ConcreteExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_ConcreteExerciseSessionEntry_WorkoutSessionID",
                table: "ConcreteExerciseSessionEntry",
                column: "WorkoutSessionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConcreteExerciseAttribute");

            migrationBuilder.DropTable(
                name: "ConcreteExerciseSessionEntry");

            migrationBuilder.DropTable(
                name: "ConcreteExercises");

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
                name: "IX_ExerciseSessionEntry_ExerciseID",
                table: "ExerciseSessionEntry",
                column: "ExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSessionEntry_WorkoutSessionID",
                table: "ExerciseSessionEntry",
                column: "WorkoutSessionID");
        }
    }
}
