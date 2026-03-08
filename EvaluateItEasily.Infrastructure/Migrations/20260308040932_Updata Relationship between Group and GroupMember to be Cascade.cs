using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvaluateItEasily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdataRelationshipbetweenGroupandGroupMembertobeCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_CreatedById",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_LeaderId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_UpdatedById",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Groups_GroupId",
                table: "Proposals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "GroupsT");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_UpdatedById",
                table: "GroupsT",
                newName: "IX_GroupsT_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_LeaderId",
                table: "GroupsT",
                newName: "IX_GroupsT_LeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CreatedById",
                table: "GroupsT",
                newName: "IX_GroupsT_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupsT",
                table: "GroupsT",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_GroupsT_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "GroupsT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsT_AspNetUsers_CreatedById",
                table: "GroupsT",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsT_AspNetUsers_LeaderId",
                table: "GroupsT",
                column: "LeaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsT_AspNetUsers_UpdatedById",
                table: "GroupsT",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_GroupsT_GroupId",
                table: "Proposals",
                column: "GroupId",
                principalTable: "GroupsT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_GroupsT_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupsT_AspNetUsers_CreatedById",
                table: "GroupsT");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupsT_AspNetUsers_LeaderId",
                table: "GroupsT");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupsT_AspNetUsers_UpdatedById",
                table: "GroupsT");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_GroupsT_GroupId",
                table: "Proposals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupsT",
                table: "GroupsT");

            migrationBuilder.RenameTable(
                name: "GroupsT",
                newName: "Groups");

            migrationBuilder.RenameIndex(
                name: "IX_GroupsT_UpdatedById",
                table: "Groups",
                newName: "IX_Groups_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_GroupsT_LeaderId",
                table: "Groups",
                newName: "IX_Groups_LeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupsT_CreatedById",
                table: "Groups",
                newName: "IX_Groups_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_CreatedById",
                table: "Groups",
                column: "CreatedById",
                principalTable: "AspNetUsers",
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
                name: "FK_Proposals_Groups_GroupId",
                table: "Proposals",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
