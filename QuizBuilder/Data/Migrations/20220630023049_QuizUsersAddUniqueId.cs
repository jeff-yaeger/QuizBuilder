using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizBuilder.Data.Migrations
{
    public partial class QuizUsersAddUniqueId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuizzes",
                table: "UserQuizzes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserQuizzes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuizzes",
                table: "UserQuizzes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizzes_UserId",
                table: "UserQuizzes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuizzes",
                table: "UserQuizzes");

            migrationBuilder.DropIndex(
                name: "IX_UserQuizzes_UserId",
                table: "UserQuizzes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserQuizzes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuizzes",
                table: "UserQuizzes",
                columns: new[] { "UserId", "QuizId" });
        }
    }
}
