using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersRestApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateNewImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_ProductEntityProductId",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "PreviewImage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "ImageEntities");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ImageEntities",
                newName: "ImageName");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ProductEntityProductId",
                table: "ImageEntities",
                newName: "IX_ImageEntities_ProductEntityProductId");

            migrationBuilder.AddColumn<string>(
                name: "PreviewImageName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageEntities_Products_ProductEntityProductId",
                table: "ImageEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageEntities",
                table: "ImageEntities");

            migrationBuilder.DropColumn(
                name: "PreviewImageName",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "ImageEntities",
                newName: "Images");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Images",
                newName: "Title");

            migrationBuilder.RenameIndex(
                name: "IX_ImageEntities_ProductEntityProductId",
                table: "Images",
                newName: "IX_Images_ProductEntityProductId");

            migrationBuilder.AddColumn<byte[]>(
                name: "PreviewImage",
                table: "Products",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Images",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_ProductEntityProductId",
                table: "Images",
                column: "ProductEntityProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
