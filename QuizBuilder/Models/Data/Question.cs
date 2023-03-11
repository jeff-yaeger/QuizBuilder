namespace QuizBuilder.Models.Data;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Enums;
using Shared;

public class Question
{
    private int? _correctAnswerCount;
    private int _wrongAnswerCount;
    public int Id { get; set; }
    public string Value { get; set; }

    [JsonIgnore]
    public virtual ICollection<Answer> Answers { get; set; }

    public int TypeId { get; set; }

    [JsonIgnore]
    public virtual QuestionType Type { get; set; }

    [JsonIgnore]
    public virtual Quiz Quiz { get; set; }

    [NotMapped]
    public int CorrectAnswerCount
    {
        get
        {
            _correctAnswerCount ??= ICorrect.Count(Answers);

            return _correctAnswerCount ?? 0;
        }
    }

    [NotMapped]
    public int IncorrectAnswerCount => Answers.Count - CorrectAnswerCount;

    [NotMapped]
    public int TotalAnswers => Answers.Count;

    [NotMapped]
    private ISet<int> CorrectAnswerIds => Answers.Where(x => x.Correct).Select(x => x.Id).ToHashSet();

    [NotMapped]
    private ISet<int> IncorrectAnswerIds => Answers.Where(x => !x.Correct).Select(x => x.Id).ToHashSet();

    public bool IsCorrectAnswer(int answerId)
    {
        return CorrectAnswerIds.Contains(answerId);
    }

    public bool IsIncorrectAnswer(int answerId)
    {
        return IncorrectAnswerIds.Contains(answerId);
    }

    public void SetType()
    {
        if (Answers == null)
        {
            return;
        }

        if (CorrectAnswerCount == 1)
        {
            TypeId = (int)QuestionTypeEnum.Single;
        }
        else
        {
            TypeId = (int)QuestionTypeEnum.Multiple;
        }
    }
}