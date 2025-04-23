using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isVisible",
                table: "blogPosts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "isVisible",
                table: "blogPosts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
