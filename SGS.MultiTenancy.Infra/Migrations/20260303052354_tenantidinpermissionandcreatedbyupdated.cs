using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGS.MultiTenancy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class tenantidinpermissionandcreatedbyupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            UPDATE sgs_mutitenancy.permissions
            SET 
                TenantID = '00000000-0000-0000-0000-000000000000',
                CreateBy = CASE 
                                WHEN CreateBy = 'System'
                                THEN '00000000-0000-0000-0000-000000000000'
                                ELSE CreateBy
                            END;
        """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            UPDATE sgs_mutitenancy.permissions
            SET 
                CreateBy = 'System'
            WHERE CreateBy = '00000000-0000-0000-0000-000000000000';
        """);
        }
    }
}
