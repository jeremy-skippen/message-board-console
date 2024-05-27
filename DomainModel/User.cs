namespace JeremySkippen.MessageBoard.DomainModel;

public record struct UserId(Guid Id)
{
    public static UserId NewId() => new(Guid.NewGuid());
};

public sealed record User(
    UserId UserId,
    string UserName
)
{
    public IReadOnlyCollection<Posting>? Postings = null;
    public IReadOnlyCollection<Project>? FollowingProjects = null;
};
