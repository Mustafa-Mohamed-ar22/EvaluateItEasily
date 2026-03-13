using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluateItEasily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFileNameCheckConditionagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Proposals_FileName_Valid",
                table: "Proposals");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Proposals_FileName_Valid",
                table: "Proposals",
                sql: "FileName NOT LIKE '%[^A-Za-z0-9_ .]%' AND FileName NOT LIKE REPLICATE('.', LEN(FileName))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Proposals_FileName_Valid",
                table: "Proposals");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Proposals_FileName_Valid",
                table: "Proposals",
                sql: "FileName NOT LIKE '%[^A-Za-z0-9_ ]%'");
        }
    }
}
