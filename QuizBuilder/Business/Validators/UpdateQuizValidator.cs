namespace QuizBuilder.Business.Validators;

using ManagerResponses;
using Models.Data;
using Models.DTO;

public class UpdateQuizValidator
{
    private readonly ApplicationUser _currentUser;
    private readonly QuizDTO _dto;
    private readonly Quiz _quiz;

    public UpdateQuizValidator(Quiz quiz, QuizDTO dto, ApplicationUser currentUser)
    {
        _quiz = quiz;
        _dto = dto;
        _currentUser = currentUser;
    }

    public IManagerResponse Validate()
    {
        var validator = new CreateQuizValidator(_dto, _currentUser);
        var response = validator.Validate();
        if (response != null)
        {
            return response;
        }

        if (_quiz == null)
        {
            return new NotFoundResponse("Quiz not found");
        }

        if (_quiz.CreatedUser.Id != _currentUser.Id)
        {
            return new BadRequestResponse("Only the user that created the quiz can edit it.");
        }

        if (_quiz.Published)
        {
            return new BadRequestResponse("Quiz is published, it cannot be edited.");
        }

        return null;
    }
}