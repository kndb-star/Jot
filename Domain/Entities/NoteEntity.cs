namespace JotBot.Domain.Entities
{
    public class NoteEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset? LastUpdatedAtUtc { get; set; }
        public string? Content { get; set; }
        public string? Tags { get; set; }
    }
}
