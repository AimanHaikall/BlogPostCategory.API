using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipsFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blogPosts_categories_Categoryid",
                table: "blogPosts");

            migrationBuilder.DropIndex(
                name: "IX_blogPosts_Categoryid",
                table: "blogPosts");

            migrationBuilder.DropColumn(
                name: "Categoryid",
                table: "blogPosts");

            migrationBuilder.CreateTable(
                name: "BlogPostCategory",
                columns: table => new
                {
                    BlogPostsid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Categoriesid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostCategory", x => new { x.BlogPostsid, x.Categoriesid });
                    table.ForeignKey(
                        name: "FK_BlogPostCategory_blogPosts_BlogPostsid",
                        column: x => x.BlogPostsid,
                        principalTable: "blogPosts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostCategory_categories_Categoriesid",
                        column: x => x.Categoriesid,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostCategory_Categoriesid",
                table: "BlogPostCategory",
                column: "Categoriesid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostCategory");

            migrationBuilder.AddColumn<Guid>(
                name: "Categoryid",
                table: "blogPosts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_blogPosts_Categoryid",
                table: "blogPosts",
                column: "Categoryid");

            migrationBuilder.AddForeignKey(
                name: "FK_blogPosts_categories_Categoryid",
                table: "blogPosts",
                column: "Categoryid",
                principalTable: "categories",
                principalColumn: "id");
        }
    }
}
