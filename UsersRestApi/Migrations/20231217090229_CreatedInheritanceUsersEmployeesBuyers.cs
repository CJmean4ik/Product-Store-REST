using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersRestApi.Migrations
{
    /// <inheritdoc />
    public partial class CreatedInheritanceUsersEmployeesBuyers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageEntities_Products_ProductEntityProductId",
                table: "ImageEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageEntities",
                table: "ImageEntities");

            migrationBuilder.RenameTable(
                name: "ImageEntities",
                newName: "Images");

            migrationBuilder.RenameIndex(
                name: "IX_ImageEntities_ProductEntityProductId",
                table: "Images",
                newName: "IX_Images_ProductEntityProductId");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "ImageId");

            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_ProductEntityProductId",
                table: "Images",
                column: "ProductEntityProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_ProductEntityProductId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "ImageEntities");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ProductEntityProductId",
                table: "ImageEntities",
                newName: "IX_ImageEntities_ProductEntityProductId");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageEntities",
                table: "ImageEntities",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageEntities_Products_ProductEntityProductId",
                table: "ImageEntities",
                column: "ProductEntityProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
