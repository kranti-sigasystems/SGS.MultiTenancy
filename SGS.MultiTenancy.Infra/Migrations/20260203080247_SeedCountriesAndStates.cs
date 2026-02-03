using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGS.MultiTenancy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class SeedCountriesAndStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Country IDs
            var indiaId = Guid.Parse("EACF2AC6-F42D-4090-A08A-39EF6CFE6978");
            var usaId = Guid.Parse("3FA155C8-90F8-48FC-A1FF-07BD4EF78FD5");
            var canadaId = Guid.Parse("565E11B2-F045-45BF-8040-23B34AC72320");

            // Insert Countries
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "ID", "Name", "Code", "Status" },
                values: new object[,]
                {
                    { indiaId,  "India", "IND", 1 },
                    { usaId,    "United States", "US", 1 },
                    { canadaId, "Canada", "CA", 1 }
                }
            );

            // Insert States
            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "ID", "CountryID", "Name", "Code", "Status" },
                values: new object[,]
                {
                    { Guid.NewGuid(), indiaId, "Maharashtra", "MH", 1 },
                    { Guid.NewGuid(), indiaId, "Gujarat", "GJ", 1 },
                    { Guid.NewGuid(), indiaId, "Karnataka", "KA", 1 },
                    { Guid.NewGuid(), indiaId, "Delhi", "DL", 1 },

                    { Guid.NewGuid(), usaId, "California", "CA", 1 },
                    { Guid.NewGuid(), usaId, "Texas", "TX", 1 },
                    { Guid.NewGuid(), usaId, "Florida", "FL", 1 },
                    { Guid.NewGuid(), usaId, "New York", "NY", 1 },

                    { Guid.NewGuid(), canadaId, "Ontario", "ON", 1 },
                    { Guid.NewGuid(), canadaId, "Quebec", "QC", 1 },
                    { Guid.NewGuid(), canadaId, "British Columbia", "BC", 1 }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete States
            migrationBuilder.DeleteData(
                table: "States",
                keyColumn: "Name",
                keyValues: new object[]
                {
                    "Maharashtra","Gujarat","Karnataka","Delhi",
                    "California","Texas","Florida","New York",
                    "Ontario","Quebec","British Columbia"
                }
            );

            // Delete Countries
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "ID",
                keyValues: new object[]
                {
                    Guid.Parse("EACF2AC6-F42D-4090-A08A-39EF6CFE6978"),
                    Guid.Parse("3FA155C8-90F8-48FC-A1FF-07BD4EF78FD5"),
                    Guid.Parse("565E11B2-F045-45BF-8040-23B34AC72320")
                }
            );
        }
    }
}
