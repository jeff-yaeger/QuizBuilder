namespace QuizBuilder.Business.Validators;

using ManagerResponses;
using Models.Data;

public class DeleteQuizValidator
{
    private readonly ApplicationUser _currentUser;
    private readonly Quiz _quiz;

    public DeleteQuizValidator(Quiz quiz, ApplicationUser currentUser)
    {
        _quiz = quiz;
        _currentUser = currentUser;
    }

    public IManagerResponse Validate()
    {
        if (_quiz == null)
        {
            return new NotFoundResponse("Quiz not found");
        }

        if (_quiz.CreatedUser.Id != _currentUser.Id)
        {
            return new BadRequestResponse("Only the user that created the quiz can delete it.");
        }

        return null;
    }
}