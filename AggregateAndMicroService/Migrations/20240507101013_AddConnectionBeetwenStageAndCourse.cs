using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AggregateAndMicroService.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionBeetwenStageAndCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stages_CourseId",
                table: "Stages",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Courses_CourseId",
                table: "Stages",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Courses_CourseId",
                table: "Stages");

            migrationBuilder.DropIndex(
                name: "IX_Stages_CourseId",
                table: "Stages");
        }
    }
}
