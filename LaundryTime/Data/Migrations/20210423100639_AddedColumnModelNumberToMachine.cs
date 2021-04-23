using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryTime.Data.Migrations
{
    public partial class AddedColumnModelNumberToMachine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelNumber",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelNumber",
                table: "Machines");
        }
    }
}
