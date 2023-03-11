using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizBuilder.Data.Migrations
{
    public partial class UserQuizAddUserAnsweredQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAnsweredQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserQuizId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnsweredQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnsweredQuestion_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnsweredQuestion_UserQuizzes_UserQuizId",
                        column: x => x.UserQuizId,
                        principalTable: "UserQuizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerUserAnsweredQuestion",
                columns: table => new
                {
                    SubmittedAnswersId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAnsweredQuestionsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerUserAnsweredQuestion", x => new { x.SubmittedAnswersId, x.UserAnsweredQuestionsId });
                    table.ForeignKey(
                        name: "FK_AnswerUserAnsweredQuestion_Answers_SubmittedAnswersId",
                        column: x => x.SubmittedAnswersId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerUserAnsweredQuestion_UserAnsweredQuestion_UserAnsweredQuestionsId",
                        column: x => x.UserAnsweredQuestionsId,
                        principalTable: "UserAnsweredQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUserAnsweredQuestion_UserAnsweredQuestionsId",
                table: "AnswerUserAnsweredQuestion",
                column: "UserAnsweredQuestionsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnsweredQuestion_QuestionId",
                table: "UserAnsweredQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnsweredQuestion_UserQuizId",
                table: "UserAnsweredQuestion",
                column: "UserQuizId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerUserAnsweredQuestion");

            migrationBuilder.DropTable(
                name: "UserAnsweredQuestion");
        }
    }
}
