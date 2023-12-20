using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExamCenterFinder.Api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZipCodeCenterPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZipCodeCenterPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExamCenters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCodeCenterPointId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamCenters_ZipCodeCenterPoints_ZipCodeCenterPointId",
                        column: x => x.ZipCodeCenterPointId,
                        principalTable: "ZipCodeCenterPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    ReservedSeats = table.Column<int>(type: "int", nullable: false),
                    ExamCenterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamSlots_ExamCenters_ExamCenterId",
                        column: x => x.ExamCenterId,
                        principalTable: "ExamCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ZipCodeCenterPoints",
                columns: new[] { "Id", "Latitude", "Longitude", "ZipCode" },
                values: new object[,]
                {
                    { 1, 45.227385700066378, -93.996024095202102, "11111" },
                    { 2, 45.561894447158792, -93.226935395477625, "22222" },
                    { 3, 44.84908604562181, -92.239981994721518, "33333" }
                });

            migrationBuilder.InsertData(
                table: "ExamCenters",
                columns: new[] { "Id", "City", "Name", "State", "StreetAddress", "ZipCodeCenterPointId" },
                values: new object[,]
                {
                    { 1, "City 1", "ABC Testing Center", "State 1", "123 Main St, Minneapolis, MN 12345", 1 },
                    { 2, "City 2", "DEF Testing Center", "State 2", "456 Side St, Minneapolis, MN 54321", 2 },
                    { 3, "City 3", "GHI Testing Center", "State 3", "789 Cross St, Minneapolis, MN 45123", 3 }
                });

            migrationBuilder.InsertData(
                table: "ExamSlots",
                columns: new[] { "Id", "Duration", "ExamCenterId", "ReservedSeats", "StartTime", "TotalSeats" },
                values: new object[,]
                {
                    { 1, 2, 1, 5, new DateTime(2023, 5, 1, 15, 0, 0, 0, DateTimeKind.Unspecified), 115 },
                    { 2, 2, 2, 10, new DateTime(2023, 5, 2, 13, 30, 0, 0, DateTimeKind.Unspecified), 115 },
                    { 3, 2, 3, 1, new DateTime(2023, 5, 1, 10, 30, 0, 0, DateTimeKind.Unspecified), 115 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamCenters_ZipCodeCenterPointId",
                table: "ExamCenters",
                column: "ZipCodeCenterPointId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSlots_ExamCenterId",
                table: "ExamSlots",
                column: "ExamCenterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamSlots");

            migrationBuilder.DropTable(
                name: "ExamCenters");

            migrationBuilder.DropTable(
                name: "ZipCodeCenterPoints");
        }
    }
}
