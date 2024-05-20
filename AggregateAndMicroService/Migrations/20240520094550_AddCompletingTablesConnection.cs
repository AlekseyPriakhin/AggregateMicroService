using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AggregateAndMicroService.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletingTablesConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StageCourseCompletings",
                table: "StageCourseCompletings");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "StageCourseCompletings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StageCourseCompletings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StageCourseCompletings",
                table: "StageCourseCompletings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StageCourseCompletings_CourseCompletingId",
                table: "StageCourseCompletings",
                column: "CourseCompletingId");

            migrationBuilder.AddForeignKey(
                name: "FK_StageCourseCompletings_CourseCompleting_CourseCompletingId",
                table: "StageCourseCompletings",
                column: "CourseCompletingId",
                principalTable: "CourseCompleting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StageCourseCompletings_CourseCompleting_CourseCompletingId",
                table: "StageCourseCompletings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StageCourseCompletings",
                table: "StageCourseCompletings");

            migrationBuilder.DropIndex(
                name: "IX_StageCourseCompletings_CourseCompletingId",
                table: "StageCourseCompletings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StageCourseCompletings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StageCourseCompletings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StageCourseCompletings",
                table: "StageCourseCompletings",
                columns: new[] { "CourseCompletingId", "StageId" });
        }
    }
}
