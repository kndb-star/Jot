namespace JotBot
{
    public record CreateNote(string Title, string Content, string? Tags);

    public record UpdateNote(string Title, string Content, string? Tags);

    public record Note(int Id, string Title, string Content, DateTimeOffset CreatedAtUtc, DateTimeOffset LastUpdatedAtUtc, string? Tags);

}
