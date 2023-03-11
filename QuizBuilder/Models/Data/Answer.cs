namespace QuizBuilder.Models.Data;

using System.Text.Json.Serialization;
using Shared;

public class Answer : ICorrect
{
    public int Id { get; set; }
    public string Value { get; set; }

    [JsonIgnore]
    public virtual Question Question { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserAnsweredQuestion> UserAnsweredQuestions { get; set; }

    public bool Correct { get; set; }
}