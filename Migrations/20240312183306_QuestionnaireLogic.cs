using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivePal.Migrations
{
    /// <inheritdoc />
    public partial class QuestionnaireLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isFlagged",
                table: "Reviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "LearnerId",
                table: "DrivingClasses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Questionnaires",
                columns: table => new
                {
                    QuestionnaireId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DrivingStatus = table.Column<int>(type: "int", nullable: false),
                    TeachingTraits = table.Column<int>(type: "int", nullable: false),
                    DrivingGoals = table.Column<int>(type: "int", nullable: false),
                    TeachingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableDaysOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeOfDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LearnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionnaires", x => x.QuestionnaireId);
                    table.ForeignKey(
                        name: "FK_Questionnaires_AspNetUsers_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrivingClasses_LearnerId",
                table: "DrivingClasses",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Questionnaires_LearnerId",
                table: "Questionnaires",
                column: "LearnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrivingClasses_AspNetUsers_LearnerId",
                table: "DrivingClasses",
                column: "LearnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrivingClasses_AspNetUsers_LearnerId",
                table: "DrivingClasses");

            migrationBuilder.DropTable(
                name: "Questionnaires");

            migrationBuilder.DropIndex(
                name: "IX_DrivingClasses_LearnerId",
                table: "DrivingClasses");

            migrationBuilder.DropColumn(
                name: "isFlagged",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "LearnerId",
                table: "DrivingClasses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
