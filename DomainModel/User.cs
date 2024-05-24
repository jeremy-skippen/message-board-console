namespace JeremySkippen.MessageBoard.DomainModel;

public record struct UserId(Guid Id)
{
    public static UserId NewId() => new(Guid.NewGuid());
};

public sealed class User
{
    public User(
        UserId userId,
        string userName
    )
    {
        UserId = userId;
        UserName = userName;
    }

    public UserId UserId { get; set; }
    public string UserName { get; set; }

    public ICollection<Posting>? Postings { get; set; }
    public ICollection<UserFollow>? UserFollowingProjects { get; set; }
    public ICollection<Project>? FollowingProjects { get; set; }
}

public sealed class UserFollow
{
    // public UserFollow(
    //     UserId userId,
    //     ProjectId projectId
    // )
    // {
    //     UserId = userId;
    //     ProjectId = projectId;
    // }

    public UserId UserId { get; set; }
    public ProjectId ProjectId { get; set; }

    public User? User { get; set; }
    public Project? Project { get; set; }
}
