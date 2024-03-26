using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivePal.Migrations
{
    /// <inheritdoc />
    public partial class messagecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorAvailableDaysOf",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructorDrivingGoals",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstructorDrivingStatus",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InstructorLessonAverage",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructorLessonDuration",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructorTeachingTraits",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructorTeachingType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructorTimeOfDay",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropColumn(
                name: "InstructorAvailableDaysOf",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstructorDrivingGoals",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstructorDrivingStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstructorLessonAverage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstructorLessonDuration",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstructorTeachingTraits",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstructorTeachingType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstructorTimeOfDay",
                table: "AspNetUsers");
        }
    }
}
