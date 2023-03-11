namespace QuizBuilder.Business.Validators;

using ManagerResponses;
using Models.Data;

public class UserSolutionsValidator
{
    private readonly ApplicationUser _currentUser;
    private readonly Quiz _quiz;
    private readonly List<UserQuiz> _userQuizzes;

    public UserSolutionsValidator(Quiz quiz, List<UserQuiz> userQuizzes, ApplicationUser currentUser)
    {
        _quiz = quiz;
        _userQuizzes = userQuizzes;
        _currentUser = currentUser;
    }

    public IManagerResponse Validate()
    {
        if (_quiz == null)
        {
            return new NotFoundResponse("Quiz not found.");
        }

        if (_quiz.CreatedUser.Id != _currentUser.Id)
        {
            return new BadRequestResponse("User does not have access to get results from this quiz.");
        }

        if (_userQuizzes.Count == 0)
        {
            return new BadRequestResponse("No one has taken this quiz");
        }

        return null;
    }
}