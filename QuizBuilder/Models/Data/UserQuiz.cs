namespace QuizBuilder.Models.Data;

using System.Text.Json.Serialization;

public class UserQuiz
{
    public int Id { get; set; }
    public string QuizId { get; set; }

    [JsonIgnore]
    public virtual Quiz Quiz { get; set; }

    public string UserId { get; set; }

    [JsonIgnore]
    public virtual ApplicationUser User { get; set; }

    public decimal Score { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserAnsweredQuestion> UserAnsweredQuestions { get; set; }
}