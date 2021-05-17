using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryTime.Migrations
{
    public partial class AddedOccupiedToMachine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Occupied",
                table: "Machines",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occupied",
                table: "Machines");
        }
    }
}
