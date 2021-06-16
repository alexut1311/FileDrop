using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace FileDrop.DAL.Migrations
{
   public partial class addedFieldsToCloudFileTable : Migration
   {
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.CreateTable(
             name: "Files",
             columns: table => new
             {
                Id = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FileSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                StorageClass = table.Column<string>(type: "nvarchar(max)", nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Files", x => x.Id);
             });
      }

      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropTable(
             name: "Files");
      }
   }
}
