namespace QuizBuilder.Models.DTO;

using System.ComponentModel.DataAnnotations;

public class QuizDTO
{
    [StringLength(1000)]
    public string? Id { get; set; }

    [StringLength(1000)]
    public string Title { get; set; }

    public List<QuestionDTO>? Questions { get; set; }
}