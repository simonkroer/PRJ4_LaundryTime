using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryTime.Data.Migrations
{
    public partial class MachineTypeToBookingList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingListModels_Machines_MachineId",
                table: "BookingListModels");

            migrationBuilder.DropColumn(
                name: "MachineName",
                table: "BookingListModels");

            migrationBuilder.AlterColumn<int>(
                name: "MachineId",
                table: "BookingListModels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingListModels_Machines_MachineId",
                table: "BookingListModels",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "MachineId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingListModels_Machines_MachineId",
                table: "BookingListModels");

            migrationBuilder.AlterColumn<int>(
                name: "MachineId",
                table: "BookingListModels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MachineName",
                table: "BookingListModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingListModels_Machines_MachineId",
                table: "BookingListModels",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "MachineId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
