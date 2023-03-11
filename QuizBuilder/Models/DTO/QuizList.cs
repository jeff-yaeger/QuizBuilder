namespace QuizBuilder.Models.DTO;

public class QuizList
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int QuestionCount { get; set; }
    public bool HasTaken { get; set; }
    public string Score { get; set; }
}