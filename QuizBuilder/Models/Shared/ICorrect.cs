namespace QuizBuilder.Models.Shared;

public interface ICorrect
{
    bool Correct { get; set; }

    public static int Count(IEnumerable<ICorrect> corrects)
    {
        return corrects == null ? 0 : corrects.Count(x => x.Correct);
    }
}