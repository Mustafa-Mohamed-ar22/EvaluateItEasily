using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluateItEasily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FirstRealMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_UpdatedById",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "Abstracts");

            migrationBuilder.AddColumn<int>(
                name: "EvaluationId",
                table: "SimilarityResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HistoricalProjectId",
                table: "SimilarityResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "SimilarityResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SimilarityScore",
                table: "SimilarityResults",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Abstract",
                table: "HistoricalProjects",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "HistoricalProjects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchivedAt",
                table: "HistoricalProjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "HistoricalProjects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "HistoricalProjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "HistoricalProjects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HistoricalProjects",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProposalId",
                table: "HistoricalProjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "HistoricalProjects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "HistoricalProjects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeaderId",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Groups",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "GroupMembers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "GroupMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "GroupMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLeader",
                table: "GroupMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedAt",
                table: "GroupMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "GroupMembers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "GroupMembers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "GroupMembers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Decisions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Decisions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DecidedAt",
                table: "Decisions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DecidedById",
                table: "Decisions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DecisionType",
                table: "Decisions",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FeedbackComment",
                table: "Decisions",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProposalId",
                table: "Decisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Decisions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Decisions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Proposals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Abstract = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    ProposalFileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proposals_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Proposals_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Proposals_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => new { x.UserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Evaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProposalId = table.Column<int>(type: "int", nullable: false),
                    EvaluatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AIStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MaxSimilarityScore = table.Column<double>(type: "float", nullable: false),
                    EvaluatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evaluations_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Evaluations_AspNetUsers_EvaluatedById",
                        column: x => x.EvaluatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Evaluations_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Evaluations_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupervisorAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProposalId = table.Column<int>(type: "int", nullable: false),
                    SupervisorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkloadNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisorAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupervisorAssignments_AspNetUsers_AssignedById",
                        column: x => x.AssignedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupervisorAssignments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupervisorAssignments_AspNetUsers_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupervisorAssignments_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupervisorAssignments_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SimilarityResults_EvaluationId_Rank",
                table: "SimilarityResults",
                columns: new[] { "EvaluationId", "Rank" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SimilarityResults_HistoricalProjectId",
                table: "SimilarityResults",
                column: "HistoricalProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricalProjects_CreatedById",
                table: "HistoricalProjects",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricalProjects_ProposalId",
                table: "HistoricalProjects",
                column: "ProposalId",
                unique: true,
                filter: "[ProposalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricalProjects_UpdatedById",
                table: "HistoricalProjects",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_LeaderId",
                table: "Groups",
                column: "LeaderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_CreatedById",
                table: "GroupMembers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupId_StudentId",
                table: "GroupMembers",
                columns: new[] { "GroupId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_StudentId",
                table: "GroupMembers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_UpdatedById",
                table: "GroupMembers",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_CreatedById",
                table: "Decisions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_DecidedById",
                table: "Decisions",
                column: "DecidedById");

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_ProposalId",
                table: "Decisions",
                column: "ProposalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decisions_UpdatedById",
                table: "Decisions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_CreatedById",
                table: "Evaluations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_EvaluatedById",
                table: "Evaluations",
                column: "EvaluatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_ProposalId",
                table: "Evaluations",
                column: "ProposalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_UpdatedById",
                table: "Evaluations",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_CreatedById",
                table: "Proposals",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_GroupId",
                table: "Proposals",
                column: "GroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_UpdatedById",
                table: "Proposals",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId1",
                table: "RefreshTokens",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorAssignments_AssignedById",
                table: "SupervisorAssignments",
                column: "AssignedById");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorAssignments_CreatedById",
                table: "SupervisorAssignments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorAssignments_ProposalId",
                table: "SupervisorAssignments",
                column: "ProposalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorAssignments_SupervisorId",
                table: "SupervisorAssignments",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorAssignments_UpdatedById",
                table: "SupervisorAssignments",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_AspNetUsers_CreatedById",
                table: "Decisions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_AspNetUsers_DecidedById",
                table: "Decisions",
                column: "DecidedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_AspNetUsers_UpdatedById",
                table: "Decisions",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_Proposals_ProposalId",
                table: "Decisions",
                column: "ProposalId",
                principalTable: "Proposals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_CreatedById",
                table: "GroupMembers",
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
                name: "FK_GroupMembers_AspNetUsers_UpdatedById",
                table: "GroupMembers",
                column: "UpdatedById",
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
                name: "FK_Groups_AspNetUsers_UpdatedById",
                table: "Groups",
                column: "UpdatedById",
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
                name: "FK_HistoricalProjects_AspNetUsers_UpdatedById",
                table: "HistoricalProjects",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricalProjects_Proposals_ProposalId",
                table: "HistoricalProjects",
                column: "ProposalId",
                principalTable: "Proposals",
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
                name: "FK_SimilarityResults_Evaluations_EvaluationId",
                table: "SimilarityResults",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SimilarityResults_HistoricalProjects_HistoricalProjectId",
                table: "SimilarityResults",
                column: "HistoricalProjectId",
                principalTable: "HistoricalProjects",
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
                name: "FK_Decisions_AspNetUsers_DecidedById",
                table: "Decisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_AspNetUsers_UpdatedById",
                table: "Decisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_Proposals_ProposalId",
                table: "Decisions");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_CreatedById",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_StudentId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_UpdatedById",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_LeaderId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_UpdatedById",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricalProjects_AspNetUsers_CreatedById",
                table: "HistoricalProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricalProjects_AspNetUsers_UpdatedById",
                table: "HistoricalProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricalProjects_Proposals_ProposalId",
                table: "HistoricalProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_SimilarityResults_Evaluations_EvaluationId",
                table: "SimilarityResults");

            migrationBuilder.DropForeignKey(
                name: "FK_SimilarityResults_HistoricalProjects_HistoricalProjectId",
                table: "SimilarityResults");

            migrationBuilder.DropTable(
                name: "Evaluations");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "SupervisorAssignments");

            migrationBuilder.DropTable(
                name: "Proposals");

            migrationBuilder.DropIndex(
                name: "IX_SimilarityResults_EvaluationId_Rank",
                table: "SimilarityResults");

            migrationBuilder.DropIndex(
                name: "IX_SimilarityResults_HistoricalProjectId",
                table: "SimilarityResults");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_HistoricalProjects_CreatedById",
                table: "HistoricalProjects");

            migrationBuilder.DropIndex(
                name: "IX_HistoricalProjects_ProposalId",
                table: "HistoricalProjects");

            migrationBuilder.DropIndex(
                name: "IX_HistoricalProjects_UpdatedById",
                table: "HistoricalProjects");

            migrationBuilder.DropIndex(
                name: "IX_Groups_LeaderId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_CreatedById",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_GroupId_StudentId",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_StudentId",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_UpdatedById",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_Decisions_CreatedById",
                table: "Decisions");

            migrationBuilder.DropIndex(
                name: "IX_Decisions_DecidedById",
                table: "Decisions");

            migrationBuilder.DropIndex(
                name: "IX_Decisions_ProposalId",
                table: "Decisions");

            migrationBuilder.DropIndex(
                name: "IX_Decisions_UpdatedById",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "EvaluationId",
                table: "SimilarityResults");

            migrationBuilder.DropColumn(
                name: "HistoricalProjectId",
                table: "SimilarityResults");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "SimilarityResults");

            migrationBuilder.DropColumn(
                name: "SimilarityScore",
                table: "SimilarityResults");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Abstract",
                table: "HistoricalProjects");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "HistoricalProjects");

            migrationBuilder.DropColumn(
                name: "ArchivedAt",
                table: "HistoricalProjects");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "HistoricalProjects");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "HistoricalProjects");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "HistoricalProjects");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "HistoricalProjects");

            migrationBuilder.DropColumn(
                name: "ProposalId",
                table: "HistoricalProjects");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "HistoricalProjects");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "HistoricalProjects");

            migrationBuilder.DropColumn(
                name: "LeaderId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "IsLeader",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "JoinedAt",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "DecidedAt",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "DecidedById",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "DecisionType",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "FeedbackComment",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "ProposalId",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Decisions");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "Abstracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abstracts", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_UpdatedById",
                table: "Groups",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
