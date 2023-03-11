namespace QuizBuilder.Models.DTO;

using System.ComponentModel.DataAnnotations;
using Shared;

public class AnswerDTO : ICorrect
{
    [StringLength(1000)]
    public string Value { get; set; }

    public bool Correct { get; set; }
}