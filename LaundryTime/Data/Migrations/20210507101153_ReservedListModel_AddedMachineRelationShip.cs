using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryTime.Data.Migrations
{
    public partial class ReservedListModel_AddedMachineRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Machine",
                table: "ReservedListModels");

            migrationBuilder.AddColumn<int>(
                name: "MachineId",
                table: "ReservedListModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReservedListModels_MachineId",
                table: "ReservedListModels",
                column: "MachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedListModels_Machines_MachineId",
                table: "ReservedListModels",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "MachineId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedListModels_Machines_MachineId",
                table: "ReservedListModels");

            migrationBuilder.DropIndex(
                name: "IX_ReservedListModels_MachineId",
                table: "ReservedListModels");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "ReservedListModels");

            migrationBuilder.AddColumn<string>(
                name: "Machine",
                table: "ReservedListModels",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
