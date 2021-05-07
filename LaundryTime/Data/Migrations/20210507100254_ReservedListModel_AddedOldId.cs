using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryTime.Data.Migrations
{
    public partial class ReservedListModel_AddedOldId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OldId",
                table: "ReservedListModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldId",
                table: "ReservedListModels");
        }
    }
}
