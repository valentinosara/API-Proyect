using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.FurtnitureStore.Data.Migrations
{
    public partial class GramaticalerrorinOrderattribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OderDate",
                table: "Orders",
                newName: "OrderDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Orders",
                newName: "OderDate");
        }
    }
}
