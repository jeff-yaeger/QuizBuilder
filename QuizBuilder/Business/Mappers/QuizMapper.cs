namespace QuizBuilder.Business.Mappers;

using Models.Data;
using Models.DTO;
using Util;

public static class QuizMapper
{
    public static Quiz Map(Quiz quiz, QuizDTO dto)
    {
        quiz.Title = dto.Title;
        if (dto.Questions.IsNullOrEmpty())
        {
            return quiz;
        }

        quiz.Questions ??= new List<Question>();
        quiz.Questions.Clear();
        dto.Questions.ForEach(questionDto => { AddQuestions(quiz, questionDto); });

        return quiz;
    }

    private static void AddQuestions(Quiz quiz, QuestionDTO questionDto)
    {
        var question = new Question
        {
            Quiz = quiz,
            Value = questionDto.Value
        };

        question.Answers ??= new List<Answer>();
        question.Answers.Clear();
        questionDto.Answers.ForEach(x =>
        {
            question.Answers.Add(new Answer
            {
                Correct = x.Correct,
                Value = x.Value,
                Question = question
            });
        });

        question.SetType();
        quiz.Questions.Add(question);
    }
}