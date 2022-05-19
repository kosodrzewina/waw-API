using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WawAPI.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventType",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Name = table.Column<string>(
                            type: "nvarchar(128)",
                            maxLength: 128,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Title = table.Column<string>(
                            type: "nvarchar(250)",
                            maxLength: 250,
                            nullable: false
                        ),
                        Description = table.Column<string>(
                            type: "nvarchar(max)",
                            maxLength: 8000,
                            nullable: false
                        ),
                        Link = table.Column<string>(
                            type: "nvarchar(1000)",
                            maxLength: 1000,
                            nullable: false
                        ),
                        Guid = table.Column<string>(
                            type: "nvarchar(1000)",
                            maxLength: 1000,
                            nullable: false
                        ),
                        IdEventType = table.Column<int>(type: "int", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_EventType_IdEventType",
                        column: x => x.IdEventType,
                        principalTable: "EventType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Events_IdEventType",
                table: "Events",
                column: "IdEventType"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Events");

            migrationBuilder.DropTable(name: "EventType");
        }
    }
}
