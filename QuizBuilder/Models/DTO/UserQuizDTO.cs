namespace QuizBuilder.Models.DTO;

using System.ComponentModel.DataAnnotations;

public class UserQuizDTO
{
    [StringLength(1000)]
    public string Id { get; set; }

    [StringLength(1000)]
    public string? Score { get; set; }

    public List<UserQuestionDTO> Questions { get; set; }

    public void RemoveDuplicateQuestions()
    {
        Questions = Questions.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
    }
}