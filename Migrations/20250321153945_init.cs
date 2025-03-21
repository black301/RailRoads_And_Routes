using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Transport__system_prototype.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false),
                    TV = table.Column<bool>(type: "bit", nullable: false),
                    AirConditioning = table.Column<bool>(type: "bit", nullable: false),
                    WiFi = table.Column<bool>(type: "bit", nullable: false),
                    Drinks = table.Column<bool>(type: "bit", nullable: false),
                    Snacks = table.Column<bool>(type: "bit", nullable: false),
                    ImgURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicleId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    TOStationId = table.Column<int>(type: "int", nullable: false),
                    FromStationId = table.Column<int>(type: "int", nullable: false),
                    TripDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Stations_FromStationId",
                        column: x => x.FromStationId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_Stations_TOStationId",
                        column: x => x.TOStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_vehicles_vehicleId",
                        column: x => x.vehicleId,
                        principalTable: "vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "ImgURL", "Location", "Name" },
                values: new object[,]
                {
                    { 1, "~/uploads/Alex.jpeg", "Alexandria, Egypt", "Alexandria" },
                    { 2, "~/uploads/Dahab.jpeg", "Dahab, Egypt", "Dahab" },
                    { 3, "~/uploads/hurghada.jpeg", "Hurghada, Egypt", "Hurghada" },
                    { 4, "~/uploads/Marsa-Alam.jpeg", "Marsa Alam, Egypt", "Marsa Alam" },
                    { 5, "~/uploads/Nuweiba.jpeg", "Nuweiba, Egypt", "Nuweiba" }
                });

            migrationBuilder.InsertData(
                table: "vehicles",
                columns: new[] { "Id", "AirConditioning", "Drinks", "ImgURL", "Name", "NumberOfSeats", "Snacks", "TV", "Type", "WiFi" },
                values: new object[,]
                {
                    { 1, true, true, "~/uploads/bus1.jpeg", "Luxury", 150, true, true, "Train", true },
                    { 2, true, false, "~/uploads/bus2.jpeg", "Standard", 200, false, false, "Train", false },
                    { 3, true, true, "~/uploads/bus3.jpeg", "Comfort", 30, true, true, "Bus", false },
                    { 4, true, true, "~/uploads/bus4.jpeg", "Premium", 55, false, true, "Bus", true }
                });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "AvailableSeats", "FromStationId", "NumberOfSeats", "Price", "TOStationId", "TripDate", "vehicleId" },
                values: new object[,]
                {
                    { 1, 10, 1, 10, 300, 3, new DateTime(2025, 3, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 50, 3, 50, 300, 5, new DateTime(2025, 3, 15, 12, 30, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 30, 2, 30, 300, 4, new DateTime(2025, 3, 16, 14, 45, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, 20, 5, 20, 300, 1, new DateTime(2025, 3, 17, 16, 0, 0, 0, DateTimeKind.Unspecified), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_FromStationId",
                table: "Trips",
                column: "FromStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TOStationId",
                table: "Trips",
                column: "TOStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_vehicleId",
                table: "Trips",
                column: "vehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "vehicles");
        }
    }
}
