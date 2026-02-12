using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGS.MultiTenancy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class statesandcountryseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var indiaId = new Guid("e3b9c783-2ae7-4659-8dbd-fd5a3ef775af");
            var usaId = new Guid("dcd518dd-700e-4778-aac4-70fb0fc4e984");
            var ukId = new Guid("fd9b701b-7702-4004-8ae7-32e01f0fb663");

            var now = DateTime.UtcNow;
            var systemUserId = new Guid("00000000-0000-0000-0000-000000000000");

            // Insert Countries
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[]
                {
                  "ID", "Name", "Code", "Status",
                  "CreateBy", "CreateOn", "LastUpdateBy", "LastUpdateOn"
                },
                values: new object[,]
                {
                     { indiaId, "India", "IN", true, systemUserId, now, null, null },
                     { usaId,   "United States", "US", true, systemUserId, now, null, null },
                     { ukId,    "United Kingdom", "UK", true, systemUserId, now, null, null }
                });

            // Insert States
            migrationBuilder.InsertData(
                table: "States",
                columns: new[]
                {
                     "ID", "CountryID", "Name", "Code", "Status",
                     "CreateBy", "CreateOn", "LastUpdateBy", "LastUpdateOn"
                },
                values: new object[,]
                {
                   // India
                   { new Guid("67d6d171-508b-4d39-a45c-ba745aefdcd3"), indiaId, "Maharashtra", "MH", true, systemUserId, now, null, null },
                   { new Guid("24def772-3fa9-43fe-a405-1e5210ff5754"), indiaId, "Gujarat", "GJ", true, systemUserId, now, null, null },
                   { new Guid("fb5ee74e-254f-4d62-8d08-39a5920648d4"), indiaId, "Karnataka", "KA", true, systemUserId, now, null, null },

                   // USA
                   { new Guid("9a50b426-7151-4488-a1b0-71a535515b4f"), usaId, "California", "CA", true, systemUserId, now, null, null },
                   { new Guid("4e5be48d-aec6-4c65-bac7-d8dbea807d90"), usaId, "Texas", "TX", true, systemUserId, now, null, null },
                   { new Guid("3cf0b74b-5561-4bc1-b919-423c50edf774"), usaId, "Florida", "FL", true, systemUserId, now, null, null },

                   // UK
                   { new Guid("b355ca92-08b4-47df-ae62-01306fa06de0"), ukId, "England", "ENG", true, systemUserId, now, null, null },
                   { new Guid("18a2700e-8f18-41d8-89ff-f156d20b4443"), ukId, "Scotland", "SCT", true, systemUserId, now, null, null },
                   { new Guid("59e0dc9b-5a35-4c0b-8774-8e99bf64ef01"), ukId, "Wales", "WLS", true, systemUserId, now, null, null },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete States first (FK dependency)
            migrationBuilder.DeleteData(
                table: "State",
                keyColumn: "ID",
                keyValues: new object[]
                {
                      new Guid("67d6d171-508b-4d39-a45c-ba745aefdcd3"),
                      new Guid("24def772-3fa9-43fe-a405-1e5210ff5754"),
                      new Guid("fb5ee74e-254f-4d62-8d08-39a5920648d4"),

                      new Guid("9a50b426-7151-4488-a1b0-71a535515b4f"),
                      new Guid("4e5be48d-aec6-4c65-bac7-d8dbea807d90"),
                      new Guid("3cf0b74b-5561-4bc1-b919-423c50edf774"),

                      new Guid("b355ca92-08b4-47df-ae62-01306fa06de0"),
                      new Guid("18a2700e-8f18-41d8-89ff-f156d20b4443"),
                      new Guid("59e0dc9b-5a35-4c0b-8774-8e99bf64ef01")
                });

            // Delete Countries
            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "ID",
                keyValues: new object[]
                {
                  new Guid("e3b9c783-2ae7-4659-8dbd-fd5a3ef775af"),
                  new Guid("dcd518dd-700e-4778-aac4-70fb0fc4e984"),
                  new Guid("fd9b701b-7702-4004-8ae7-32e01f0fb663")
                });
        }
    }
}
