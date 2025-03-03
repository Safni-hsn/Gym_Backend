using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTrainerIdFromTrainingPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "TrainingPlans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrainerId",
                table: "TrainingPlans",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
