using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG7312_POE.Migrations
{
    /// <inheritdoc />
    public partial class userFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Services",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Services_UserID",
                table: "Services",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Users_UserID",
                table: "Services",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Users_UserID",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_UserID",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Services");
        }
    }
}
