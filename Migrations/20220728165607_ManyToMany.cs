using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WawAPI.Migrations
{
    public partial class ManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventTypes_IdEventType",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_IdEventType",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IdEventType",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "EventEventType",
                columns: table => new
                {
                    EventsId = table.Column<int>(type: "int", nullable: false),
                    TypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEventType", x => new { x.EventsId, x.TypesId });
                    table.ForeignKey(
                        name: "FK_EventEventType_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventEventType_EventTypes_TypesId",
                        column: x => x.TypesId,
                        principalTable: "EventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventEventType_TypesId",
                table: "EventEventType",
                column: "TypesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventEventType");

            migrationBuilder.AddColumn<int>(
                name: "IdEventType",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Events_IdEventType",
                table: "Events",
                column: "IdEventType");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventTypes_IdEventType",
                table: "Events",
                column: "IdEventType",
                principalTable: "EventTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
