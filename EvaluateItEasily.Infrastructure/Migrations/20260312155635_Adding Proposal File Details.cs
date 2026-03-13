using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluateItEasily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingProposalFileDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_AspNetUsers_CreatedById",
                table: "Decisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_AspNetUsers_CreatedById",
                table: "Evaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_StudentId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_LeaderId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricalProjects_AspNetUsers_CreatedById",
                table: "HistoricalProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_AspNetUsers_CreatedById",
                table: "Proposals");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Groups_GroupId",
                table: "Proposals");

            migrationBuilder.DropForeignKey(
                name: "FK_SupervisorAssignments_AspNetUsers_CreatedById",
                table: "SupervisorAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_SupervisorAssignments_AspNetUsers_SupervisorId",
                table: "SupervisorAssignments");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Proposals",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "Proposals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Proposals",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoredFileName",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Proposals_FileName_Valid",
                table: "Proposals",
                sql: "FileName NOT LIKE '%[^A-Za-z0-9_]%'");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_AspNetUsers_CreatedById",
                table: "Decisions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_AspNetUsers_CreatedById",
                table: "Evaluations",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_StudentId",
                table: "GroupMembers",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_LeaderId",
                table: "Groups",
                column: "LeaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricalProjects_AspNetUsers_CreatedById",
                table: "HistoricalProjects",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_AspNetUsers_CreatedById",
                table: "Proposals",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_Groups_GroupId",
                table: "Proposals",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupervisorAssignments_AspNetUsers_CreatedById",
                table: "SupervisorAssignments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupervisorAssignments_AspNetUsers_SupervisorId",
                table: "SupervisorAssignments",
                column: "SupervisorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_AspNetUsers_CreatedById",
                table: "Decisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_AspNetUsers_CreatedById",
                table: "Evaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_StudentId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_LeaderId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricalProjects_AspNetUsers_CreatedById",
                table: "HistoricalProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_AspNetUsers_CreatedById",
                table: "Proposals");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Groups_GroupId",
                table: "Proposals");

            migrationBuilder.DropForeignKey(
                name: "FK_SupervisorAssignments_AspNetUsers_CreatedById",
                table: "SupervisorAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_SupervisorAssignments_AspNetUsers_SupervisorId",
                table: "SupervisorAssignments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Proposals_FileName_Valid",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "StoredFileName",
                table: "Proposals");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_AspNetUsers_CreatedById",
                table: "Decisions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_AspNetUsers_CreatedById",
                table: "Evaluations",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_StudentId",
                table: "GroupMembers",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_LeaderId",
                table: "Groups",
                column: "LeaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricalProjects_AspNetUsers_CreatedById",
                table: "HistoricalProjects",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_AspNetUsers_CreatedById",
                table: "Proposals",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_Groups_GroupId",
                table: "Proposals",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupervisorAssignments_AspNetUsers_CreatedById",
                table: "SupervisorAssignments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupervisorAssignments_AspNetUsers_SupervisorId",
                table: "SupervisorAssignments",
                column: "SupervisorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
