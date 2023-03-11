namespace QuizBuilder.Business.Interfaces;

public interface ITokenManager
{
    string CreateToken(string userId);
}