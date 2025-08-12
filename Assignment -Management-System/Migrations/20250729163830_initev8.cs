using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment__Management_System.Migrations
{
    /// <inheritdoc />
    public partial class initev8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_PhoneNumber_Format",
                table: "Users",
                sql: "PhoneNumber NOT LIKE '%[^0-9]%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_PhoneNumber_Format",
                table: "Users");
        }
    }
}
