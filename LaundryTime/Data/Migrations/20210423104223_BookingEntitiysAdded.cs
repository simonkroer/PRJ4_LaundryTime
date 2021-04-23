using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryTime.Data.Migrations
{
    public partial class BookingEntitiysAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelNumber",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DateModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingListModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineName = table.Column<int>(type: "int", nullable: false),
                    MachineId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingListModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingListModels_DateModels_DateModelId",
                        column: x => x.DateModelId,
                        principalTable: "DateModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingListModels_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "MachineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservedListModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Machine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedListModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservedListModels_DateModels_DateModelId",
                        column: x => x.DateModelId,
                        principalTable: "DateModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingListModels_DateModelId",
                table: "BookingListModels",
                column: "DateModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingListModels_MachineId",
                table: "BookingListModels",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedListModels_DateModelId",
                table: "ReservedListModels",
                column: "DateModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingListModels");

            migrationBuilder.DropTable(
                name: "ReservedListModels");

            migrationBuilder.DropTable(
                name: "DateModels");

            migrationBuilder.DropColumn(
                name: "ModelNumber",
                table: "Machines");
        }
    }
}
