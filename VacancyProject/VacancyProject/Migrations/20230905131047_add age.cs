using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacancyProject.Migrations
{
    /// <inheritdoc />
    public partial class addage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Jobseekers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Jobseekers");
        }
    }
}
