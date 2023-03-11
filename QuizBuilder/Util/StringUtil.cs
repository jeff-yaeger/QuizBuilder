namespace QuizBuilder.Util;

public static class StringUtil
{
    public static string Join(this IEnumerable<string> values, string divider)
    {
        return string.Join(divider, values.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}