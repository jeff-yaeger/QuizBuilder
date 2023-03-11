using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizBuilder.Data.Migrations
{
    public partial class ChangeScoreToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "UserQuizzes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<decimal>(
                name: "Score",
                table: "UserAnsweredQuestion",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "UserAnsweredQuestion");

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "UserQuizzes",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }
    }
}
