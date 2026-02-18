using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGS.MultiTenancy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Convert column type from bit to int
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            // 2. Normalize existing values
            migrationBuilder.Sql(@"
            UPDATE Users
            SET Status = CASE 
                WHEN Status = 1 THEN 1  -- Active
                ELSE 2                  -- Inactive
            END
        ");

            // 3. Add CHECK constraint to enforce enum integrity
            migrationBuilder.Sql(@"
            ALTER TABLE Users
            ADD CONSTRAINT CK_Users_Status
            CHECK (Status IN (1,2,3,4))
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove constraint
            migrationBuilder.Sql(@"
            ALTER TABLE Users
            DROP CONSTRAINT CK_Users_Status
        ");

            // Convert int back to bit
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // Normalize values for rollback
            migrationBuilder.Sql(@"
            UPDATE Users
            SET Status = CASE 
                WHEN Status = 1 THEN 1
                ELSE 0
            END
        ");
        }
    }
}
