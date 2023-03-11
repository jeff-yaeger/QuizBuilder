namespace QuizBuilder.Models.DTO;

using System.ComponentModel.DataAnnotations;
using Util;

public class UserQuestionDTO
{
    public int Id { get; set; }
    public ICollection<int>? Answers { get; set; }

    [StringLength(1000)]
    public string? Score { get; set; }

    public void RemoveDuplicateAnswers()
    {
        Answers = Answers.Distinct().ToList();
    }

    public bool Skipped()
    {
        return Answers.IsNullOrEmpty();
    }
}