using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryTime.Data.Migrations
{
    public partial class date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "DateModels",
                newName: "DateData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateData",
                table: "DateModels",
                newName: "Date");
        }
    }
}
