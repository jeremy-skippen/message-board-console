namespace JeremySkippen.MessageBoard.DomainModel;

public record struct PostingId(Guid Id)
{
    public static PostingId NewId() => new(Guid.NewGuid());
};

public sealed record Posting(
    PostingId PostingId,
    UserId PostedById,
    ProjectId ProjectId,
    string Message,
    DateTimeOffset PostingDateTime,
    User? PostedBy = null,
    Project? Project = null
);
