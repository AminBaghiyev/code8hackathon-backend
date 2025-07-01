using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Management.DL.Migrations
{
    /// <inheritdoc />
    public partial class ThumbnailAddedToRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "767b9ec5-6a12-4ee8-9169-dfa0ef38eaab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54200bee-836f-4909-8db1-45068f41ac4f", "AQAAAAIAAYagAAAAEKRSF0C8hXNiepOvYfJ8V7d8XmOIj3qfNZx9k/9Ah7CPwLrEHSar+so8fGeG0TAC1w==", "3019f3c6-6f91-406d-9852-6a5a7c218bd4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Rooms");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "767b9ec5-6a12-4ee8-9169-dfa0ef38eaab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b08f6028-b4f7-4f2a-8c47-4efbb0c76e0d", "AQAAAAIAAYagAAAAEKbIPEwb8ZgUJXOAtIzKP+Ov2+H1LW+Fbs0gKi0XLPHU1QWecgMwRX/FHOXWoB0+tA==", "033d34b2-b782-4d2c-b300-c6fb80f64e3b" });
        }
    }
}
