using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HMS.Migrations
{
    /// <inheritdoc />
    public partial class migration11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminCategories_AdminRooms_AdminRoomId",
                table: "AdminCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminServiceAddons_AdminRooms_AdminRoomId",
                table: "AdminServiceAddons");

            migrationBuilder.DropTable(
                name: "AdminAdditionalInfo");

            migrationBuilder.DropIndex(
                name: "IX_AdminCategories_AdminRoomId",
                table: "AdminCategories");

            migrationBuilder.DropColumn(
                name: "DisplaySizePath",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "AdminRoomId",
                table: "AdminCategories");

            migrationBuilder.DropColumn(
                name: "Values",
                table: "AdminCategories");

            migrationBuilder.RenameColumn(
                name: "ThumbNailPath",
                table: "Images",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FullSizePath",
                table: "Images",
                newName: "FilePath");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Rooms",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Images",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Contacts",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "AditionalInfoDescription",
                table: "AdminRooms",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AditionalInfoTitle",
                table: "AdminRooms",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "CoverImageId",
                table: "AdminRooms",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "AdminRooms",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AdminCategories",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "AdminBlogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subtitle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tags = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BlogContent = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuthorName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuthorDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Facebook = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Twitter = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LinkedIn = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminBlogs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdminCategoryValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdminCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminCategoryValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminCategoryValues_AdminCategories_AdminCategoryId",
                        column: x => x.AdminCategoryId,
                        principalTable: "AdminCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdminContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PageTitle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PageDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressLine1 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressLine2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StateProvince = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ZipCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminContacts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoryValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AdminCategoryValuesId = table.Column<int>(type: "int", nullable: false),
                    AdminRoomId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryValues_AdminCategoryValues_AdminCategoryValuesId",
                        column: x => x.AdminCategoryValuesId,
                        principalTable: "AdminCategoryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryValues_AdminRooms_AdminRoomId",
                        column: x => x.AdminRoomId,
                        principalTable: "AdminRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AdminCategoryValues_AdminCategoryId",
                table: "AdminCategoryValues",
                column: "AdminCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryValues_AdminCategoryValuesId",
                table: "CategoryValues",
                column: "AdminCategoryValuesId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryValues_AdminRoomId",
                table: "CategoryValues",
                column: "AdminRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminServiceAddons_AdminRooms_AdminRoomId",
                table: "AdminServiceAddons",
                column: "AdminRoomId",
                principalTable: "AdminRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminServiceAddons_AdminRooms_AdminRoomId",
                table: "AdminServiceAddons");

            migrationBuilder.DropTable(
                name: "AdminBlogs");

            migrationBuilder.DropTable(
                name: "AdminContacts");

            migrationBuilder.DropTable(
                name: "CategoryValues");

            migrationBuilder.DropTable(
                name: "AdminCategoryValues");

            migrationBuilder.DropColumn(
                name: "AditionalInfoDescription",
                table: "AdminRooms");

            migrationBuilder.DropColumn(
                name: "AditionalInfoTitle",
                table: "AdminRooms");

            migrationBuilder.DropColumn(
                name: "CoverImageId",
                table: "AdminRooms");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "AdminRooms");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Images",
                newName: "ThumbNailPath");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Images",
                newName: "FullSizePath");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Rooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Images",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "DisplaySizePath",
                table: "Images",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Images",
                type: "longblob",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Contacts",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AdminCategories",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminRoomId",
                table: "AdminCategories",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Values",
                table: "AdminCategories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdminAdditionalInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AdminRoomId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Adons = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAdditionalInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminAdditionalInfo_AdminRooms_AdminRoomId",
                        column: x => x.AdminRoomId,
                        principalTable: "AdminRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AdminCategories_AdminRoomId",
                table: "AdminCategories",
                column: "AdminRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminAdditionalInfo_AdminRoomId",
                table: "AdminAdditionalInfo",
                column: "AdminRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminCategories_AdminRooms_AdminRoomId",
                table: "AdminCategories",
                column: "AdminRoomId",
                principalTable: "AdminRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminServiceAddons_AdminRooms_AdminRoomId",
                table: "AdminServiceAddons",
                column: "AdminRoomId",
                principalTable: "AdminRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
