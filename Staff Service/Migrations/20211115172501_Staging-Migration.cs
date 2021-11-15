using Microsoft.EntityFrameworkCore.Migrations;

namespace Staff_Service.Migrations
{
    public partial class StagingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__staff",
                schema: "staff",
                table: "_staff");

            migrationBuilder.EnsureSchema(
                name: "staging");

            migrationBuilder.RenameTable(
                name: "_staff",
                schema: "staff",
                newName: "staging_db",
                newSchema: "staging");

            migrationBuilder.AddPrimaryKey(
                name: "PK_staging_db",
                schema: "staging",
                table: "staging_db",
                column: "StaffID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_staging_db",
                schema: "staging",
                table: "staging_db");

            migrationBuilder.EnsureSchema(
                name: "staff");

            migrationBuilder.RenameTable(
                name: "staging_db",
                schema: "staging",
                newName: "_staff",
                newSchema: "staff");

            migrationBuilder.AddPrimaryKey(
                name: "PK__staff",
                schema: "staff",
                table: "_staff",
                column: "StaffID");
        }
    }
}
