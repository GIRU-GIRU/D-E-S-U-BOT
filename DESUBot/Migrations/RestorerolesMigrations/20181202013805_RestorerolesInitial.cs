using Microsoft.EntityFrameworkCore.Migrations;

namespace DESUBot.Migrations.RestorerolesMigrations
{
    public partial class RestorerolesInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestoreRoles",
                columns: table => new
                {
                    UserID = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(nullable: true),
                    AdminID = table.Column<ulong>(nullable: false),
                    DateInserted = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestoreRoles", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "RoleModelRolesStore",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<ulong>(nullable: false),
                    RoleName = table.Column<string>(nullable: true),
                    RoleID = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleModelRolesStore", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestoreRoles");

            migrationBuilder.DropTable(
                name: "RoleModelRolesStore");
        }
    }
}
