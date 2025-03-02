using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedvehicleIddriverIdinongoinglogistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverContact",
                table: "OngoingLogistics");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "OngoingLogistics");

            migrationBuilder.DropColumn(
                name: "VehicleImagePath",
                table: "OngoingLogistics");

            migrationBuilder.DropColumn(
                name: "VehicleName",
                table: "OngoingLogistics");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "OngoingLogistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "OngoingLogistics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "OngoingLogistics");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "OngoingLogistics");

            migrationBuilder.AddColumn<string>(
                name: "DriverContact",
                table: "OngoingLogistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "OngoingLogistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleImagePath",
                table: "OngoingLogistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleName",
                table: "OngoingLogistics",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
