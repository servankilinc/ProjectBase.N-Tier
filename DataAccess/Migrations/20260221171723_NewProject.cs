using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Archives",
                table: "Archives");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "ProjectLogs");

            migrationBuilder.RenameTable(
                name: "Archives",
                newName: "ProjectArchives");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "dbo_refreshToken",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ClientType",
                table: "dbo_refreshToken",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "dbo_refreshToken",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "dbo_refreshToken",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<byte>(
                name: "Action",
                table: "ProjectLogs",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "Action",
                table: "ProjectArchives",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectLogs",
                table: "ProjectLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectArchives",
                table: "ProjectArchives",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectLogs",
                table: "ProjectLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectArchives",
                table: "ProjectArchives");

            migrationBuilder.DropColumn(
                name: "ClientType",
                table: "dbo_refreshToken");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "dbo_refreshToken");

            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "dbo_refreshToken");

            migrationBuilder.RenameTable(
                name: "ProjectLogs",
                newName: "Logs");

            migrationBuilder.RenameTable(
                name: "ProjectArchives",
                newName: "Archives");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "dbo_refreshToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Action",
                table: "Logs",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "Action",
                table: "Archives",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Archives",
                table: "Archives",
                column: "Id");
        }
    }
}
