using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace babylawya.Migrations
{
    public partial class pathtoname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Keywords_Documents_DocumentId1",
                table: "Keywords");

            migrationBuilder.DropIndex(
                name: "IX_Keywords_DocumentId1",
                table: "Keywords");

            migrationBuilder.DropColumn(
                name: "DocumentId1",
                table: "Keywords");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Documents",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Documents",
                newName: "Path");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId1",
                table: "Keywords",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_DocumentId1",
                table: "Keywords",
                column: "DocumentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Keywords_Documents_DocumentId1",
                table: "Keywords",
                column: "DocumentId1",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
