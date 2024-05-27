namespace JeremySkippen.MessageBoard.DomainModel;

public record struct ProjectId(Guid Id)
{
    public static ProjectId NewId() => new(Guid.NewGuid());
};

public sealed record Project(
    ProjectId ProjectId,
    string ProjectName
)
{
    public IReadOnlyCollection<Posting>? Postings = null;
    public IReadOnlyCollection<User>? FollowedByUsers = null;
}
