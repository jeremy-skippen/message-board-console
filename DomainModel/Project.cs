namespace JeremySkippen.MessageBoard.DomainModel;

public record struct ProjectId(Guid Id)
{
    public static ProjectId NewId() => new(Guid.NewGuid());
};

public sealed class Project
{
    public Project(
        ProjectId projectId,
        string projectName
    )
    {
        ProjectId = projectId;
        ProjectName = projectName;
    }

    public ProjectId ProjectId { get; set; }
    public string ProjectName { get; set; }

    public ICollection<Posting>? Postings { get; set; }
    public ICollection<UserFollow>? ProjectFollowedByUsers { get; set; }
    public ICollection<User>? FollowedByUsers { get; set; }
}
