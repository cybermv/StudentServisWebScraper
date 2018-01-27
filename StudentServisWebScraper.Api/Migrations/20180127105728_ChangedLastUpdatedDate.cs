using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace StudentServisWebScraper.Api.Migrations
{
    public partial class ChangedLastUpdatedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateLastChanged",
                table: "JobOffers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateLastChanged",
                table: "JobOffers",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
