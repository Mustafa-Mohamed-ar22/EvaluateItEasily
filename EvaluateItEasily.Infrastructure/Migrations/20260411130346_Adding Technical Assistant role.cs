using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluateItEasily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingTechnicalAssistantrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TechnicalAssistantId",
                table: "SupervisorAssignments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorAssignments_TechnicalAssistantId",
                table: "SupervisorAssignments",
                column: "TechnicalAssistantId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupervisorAssignments_AspNetUsers_TechnicalAssistantId",
                table: "SupervisorAssignments",
                column: "TechnicalAssistantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupervisorAssignments_AspNetUsers_TechnicalAssistantId",
                table: "SupervisorAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SupervisorAssignments_TechnicalAssistantId",
                table: "SupervisorAssignments");

            migrationBuilder.DropColumn(
                name: "TechnicalAssistantId",
                table: "SupervisorAssignments");
        }
    }
}
