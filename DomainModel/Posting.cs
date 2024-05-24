namespace JeremySkippen.MessageBoard.DomainModel;

public record struct PostingId(Guid Id)
{
    public static PostingId NewId() => new(Guid.NewGuid());
};

public sealed class Posting
{
    public Posting(
        PostingId postingId,
        UserId postedById,
        ProjectId projectId,
        string message,
        DateTimeOffset postingDateTime
    )
    {
        PostingId = postingId;
        PostedById = postedById;
        ProjectId = projectId;
        Message = message;
        PostingDateTime = postingDateTime;
    }

    public PostingId PostingId { get; set; }
    public UserId PostedById { get; set; }
    public ProjectId ProjectId { get; set; }
    public string Message { get; set; }
    public DateTimeOffset PostingDateTime { get; set; }

    public User? PostedBy { get; set; }

    public Project? Project { get; set; }
}
