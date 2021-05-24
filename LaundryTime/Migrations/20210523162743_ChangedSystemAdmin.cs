using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryTime.Migrations
{
    public partial class ChangedSystemAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentUserAdminName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentUserAdminName",
                table: "AspNetUsers");
        }
    }
}
