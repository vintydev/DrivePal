using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivePal.Migrations
{
    /// <inheritdoc />
    public partial class QuestionnaireLogicUpdate6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayIndex",
                table: "Questionnaires");

            migrationBuilder.DropColumn(
                name: "GoalIndex",
                table: "Questionnaires");

            migrationBuilder.DropColumn(
                name: "TraitIndex",
                table: "Questionnaires");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayIndex",
                table: "Questionnaires",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GoalIndex",
                table: "Questionnaires",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TraitIndex",
                table: "Questionnaires",
                type: "int",
                nullable: true);
        }
    }
}
