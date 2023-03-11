namespace QuizBuilder.Models.Data;

using System.Text.Json.Serialization;

public class UserAnsweredQuestion
{
    public int Id { get; set; }

    [JsonIgnore]
    public virtual ICollection<Answer> SubmittedAnswers { get; set; }

    [JsonIgnore]
    public virtual Question Question { get; set; }

    [JsonIgnore]
    public virtual UserQuiz UserQuiz { get; set; }

    public decimal Score { get; set; }
}