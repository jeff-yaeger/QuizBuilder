namespace QuizBuilder.Business.Validators;

using ManagerResponses;
using Models.Data;

public class SubmitQuizValidator
{
    private readonly Quiz _quiz;
    private readonly UserQuiz _submittedQuiz;

    public SubmitQuizValidator(Quiz quiz, UserQuiz submittedQuiz)
    {
        _quiz = quiz;
        _submittedQuiz = submittedQuiz;
    }

    public IManagerResponse Validate()
    {
        if (_submittedQuiz != null)
        {
            return new BadRequestResponse("User cannot take quiz twice");
        }


        if (_quiz == null)
        {
            return new NotFoundResponse("Quiz not found");
        }

        if (!_quiz.Published)
        {
            return new BadRequestResponse("Quiz is not published and cannot be taken.");
        }

        return null;
    }
}