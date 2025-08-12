using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment__Management_System.Migrations
{
    /// <inheritdoc />
    public partial class initev7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeOfAssignment",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeOfAssignment",
                table: "Assignments");
        }
    }
}
