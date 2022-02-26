using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WawAPI.Migrations
{
    public partial class EventTypeDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventType_IdEventType",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventType",
                table: "EventType");

            migrationBuilder.RenameTable(
                name: "EventType",
                newName: "EventTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTypes",
                table: "EventTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventTypes_IdEventType",
                table: "Events",
                column: "IdEventType",
                principalTable: "EventTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventTypes_IdEventType",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTypes",
                table: "EventTypes");

            migrationBuilder.RenameTable(
                name: "EventTypes",
                newName: "EventType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventType",
                table: "EventType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventType_IdEventType",
                table: "Events",
                column: "IdEventType",
                principalTable: "EventType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
