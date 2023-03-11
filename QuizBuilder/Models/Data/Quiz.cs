namespace QuizBuilder.Models.Data;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Util;

public class Quiz
{
    public string Id { get; set; }
    public string Title { get; set; }

    public bool Published { get; set; }

    [JsonIgnore]
    public virtual ICollection<Question> Questions { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserQuiz> UserQuizzes { get; set; }

    [JsonIgnore]
    public virtual ApplicationUser CreatedUser { get; set; }

    [NotMapped]
    public int TotalPossiblePoints => Questions.IsNullOrEmpty() ? 0 : Questions.Count;

    public DateTime CreatedDate { get; set; }

    public void SetPublished()
    {
        Published = Questions != null && Questions.Count > 0;
    }
}