using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AggregateAndMicroService.Migrations
{
    /// <inheritdoc />
    public partial class addOrderToStage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Stages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Stages");
        }
    }
}
