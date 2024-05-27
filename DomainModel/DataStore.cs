namespace JeremySkippen.MessageBoard.DomainModel;

public sealed class DataStore
{
    private readonly ICollection<Posting> _posts;
    private readonly ICollection<Project> _projects;
    private readonly ICollection<User> _users;

    public DataStore()
    {
        _posts = new HashSet<Posting>();
        _projects = new HashSet<Project>();
        _users = new HashSet<User>();
    }

    #region Posting Operations

    public void AddPosting(Posting posting)
    {
        var project = FindProjectByIdInternal(posting.ProjectId);
        var postedBy = FindUserByIdInternal(posting.PostedById);

        var newPost = posting with { PostedBy = postedBy, Project = project };

        _posts.Add(newPost);

        if (project is not null)
            project.Postings = project.Postings is null
                ? new HashSet<Posting> { newPost }
                : new HashSet<Posting>(project.Postings.Append(newPost));

        if (postedBy is not null)
            postedBy.Postings = postedBy.Postings is null
                ? new HashSet<Posting> { newPost }
                : new HashSet<Posting>(postedBy.Postings.Append(newPost));
    }

    public IReadOnlyList<Posting> GetPostingsByProjectId(ProjectId projectId)
        => _posts
            .Where(p => p.ProjectId == projectId)
            .Select(p => p with
            {
                Project = null,
                PostedBy = p.PostedBy is null
                    ? null
                    : p.PostedBy with
                    {
                        FollowingProjects = null,
                        Postings = null
                    }
            })
            .ToList();

    #endregion

    #region Project Operations

    public void AddProject(Project project)
    {
        if (FindProjectByIdInternal(project.ProjectId) is not null)
            throw new InvalidOperationException($"Project with id {project.ProjectId} already exists");

        _projects.Add(project with { FollowedByUsers = new HashSet<User>(), Postings = new HashSet<Posting>() });
    }

    private Project? FindProjectByIdInternal(ProjectId projectId)
        => _projects.SingleOrDefault(p => p.ProjectId == projectId);

    public Project? FindProjectById(ProjectId projectId)
    {
        var project = FindProjectByIdInternal(projectId);
        if (project is null)
            return null;
        
        return project with { };
    }

    public Project? FindProjectByName(string projectName)
    {
        var project = _projects
            .Where(p => p.ProjectName.Equals(projectName, StringComparison.InvariantCultureIgnoreCase))
            .FirstOrDefault();
        if (project is null)
            return null;
        
        return project with { };
    }

    #endregion

    #region User Operations

    public void AddUser(User user)
    {
        if (FindUserByIdInternal(user.UserId) is not null)
            throw new InvalidOperationException($"User with id {user.UserId} already exists");

        _users.Add(user with { FollowingProjects = new HashSet<Project>(), Postings = new HashSet<Posting>() });
    }

    private User? FindUserByIdInternal(UserId userId)
        => _users.SingleOrDefault(u => u.UserId == userId);

    public User? FindUserById(UserId userId)
    {
        var user = FindUserByIdInternal(userId);
        if (user is null)
            return null;
        
        return user with { };
    }

    public User? FindUserByName(string userName)
    {
        var user = _users
            .Where(p => p.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase))
            .FirstOrDefault();
        if (user is null)
            return null;
        
        return user with { };
    }

    #endregion
}
