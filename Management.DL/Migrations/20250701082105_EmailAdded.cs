using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Management.DL.Migrations
{
    /// <inheritdoc />
    public partial class EmailAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "767b9ec5-6a12-4ee8-9169-dfa0ef38eaab",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b08f6028-b4f7-4f2a-8c47-4efbb0c76e0d", true, "BAGHIYEV.AMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEKbIPEwb8ZgUJXOAtIzKP+Ov2+H1LW+Fbs0gKi0XLPHU1QWecgMwRX/FHOXWoB0+tA==", "033d34b2-b782-4d2c-b300-c6fb80f64e3b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "767b9ec5-6a12-4ee8-9169-dfa0ef38eaab",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "25256dbd-6729-46c7-888e-37a5f47df4b5", false, null, "AQAAAAIAAYagAAAAECm7Ia0gijemcCh6z+cpkjTLgsdjkcC+PLX0YLG6h9iIZoILfQfiwE8pd8ZdtbJoLA==", "fbc0e12f-7def-4c2b-8fd2-ed04d343a3cf" });
        }
    }
}
