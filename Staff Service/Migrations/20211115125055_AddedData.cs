using Microsoft.EntityFrameworkCore.Migrations;

namespace Staff_Service.Migrations
{
    public partial class AddedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "staff");

            migrationBuilder.RenameTable(
                name: "_staff",
                newName: "_staff",
                newSchema: "staff");

            migrationBuilder.InsertData(
                schema: "staff",
                table: "_staff",
                columns: new[] { "StaffID", "StaffEmailAddress", "StaffFirstName", "StaffLastName" },
                values: new object[] { 1, "Jacob-Jardine@ThAmCo.co.uk", "Jacob", "Jardine" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "staff",
                table: "_staff",
                keyColumn: "StaffID",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "_staff",
                schema: "staff",
                newName: "_staff");
        }
    }
}
