using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivePal.Migrations
{
    /// <inheritdoc />
    public partial class AddChatGroups : Migration
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
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrivingClasses_LearnerId",
                table: "DrivingClasses",
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
                name: "ChatMessages");

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
