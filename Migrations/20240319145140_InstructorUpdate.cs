using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivePal.Migrations
{
    /// <inheritdoc />
    public partial class InstructorUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AveragePricePerLesson",
                table: "AspNetUsers",
                newName: "InstructorLessonAverage");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.RenameColumn(
                name: "InstructorLessonAverage",
                table: "AspNetUsers",
                newName: "AveragePricePerLesson");
        }
    }
}
