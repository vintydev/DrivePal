using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivePal.Migrations
{
    /// <inheritdoc />
    public partial class QuestionnaireLogicUpdate7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AveragePricePerLesson",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AveragePricePerLesson",
                table: "AspNetUsers");
        }
    }
}
