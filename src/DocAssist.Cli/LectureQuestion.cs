public static class LectureQuestion
{
    public static string? Depuis(string[] args)
    {
        string question = string.Join(
            " ",
            args.Select(argument => argument.Trim())
                .Where(argument => !string.IsNullOrWhiteSpace(argument)));

        return string.IsNullOrWhiteSpace(question) ? null : question;
    }
}
