namespace QuizBuilder.Models.DTO;

using System.ComponentModel.DataAnnotations;
using Shared;

public class QuestionDTO
{
    private int? _correctAnswerCount;

    [StringLength(1000)]
    public string Value { get; set; }

    public List<AnswerDTO>? Answers { get; set; }

    public int CorrectAnswerCount
    {
        get
        {
            _correctAnswerCount ??= ICorrect.Count(Answers);

            return _correctAnswerCount ?? 0;
        }
    }
}