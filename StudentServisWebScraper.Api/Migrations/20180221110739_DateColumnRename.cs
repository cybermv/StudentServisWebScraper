using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace StudentServisWebScraper.Api.Migrations
{
    public partial class DateColumnRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateLastChanged",
                table: "JobOffers",
                newName: "DateLastSeen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateLastSeen",
                table: "JobOffers",
                newName: "DateLastChanged");
        }
    }
}
