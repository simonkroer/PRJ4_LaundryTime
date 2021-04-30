using Microsoft.EntityFrameworkCore.Migrations;

namespace LaundryTime.Migrations
{
    public partial class LaundyLogAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaundryLog_AspNetUsers_LogId",
                table: "LaundryLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaundryLog",
                table: "LaundryLog");

            migrationBuilder.RenameTable(
                name: "LaundryLog",
                newName: "LaundryLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaundryLogs",
                table: "LaundryLogs",
                column: "LogId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaundryLogs_AspNetUsers_LogId",
                table: "LaundryLogs",
                column: "LogId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaundryLogs_AspNetUsers_LogId",
                table: "LaundryLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaundryLogs",
                table: "LaundryLogs");

            migrationBuilder.RenameTable(
                name: "LaundryLogs",
                newName: "LaundryLog");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaundryLog",
                table: "LaundryLog",
                column: "LogId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaundryLog_AspNetUsers_LogId",
                table: "LaundryLog",
                column: "LogId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
