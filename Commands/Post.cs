using JeremySkippen.MessageBoard.DomainModel;
using JeremySkippen.MessageBoard.Posts;
using JeremySkippen.MessageBoard.Projects;
using JeremySkippen.MessageBoard.Users;
using MediatR;

namespace JeremySkippen.MessageBoard.Commands;

public sealed record Post(string UserName, string ProjectName, string Message) : IRequest;

public sealed class PostHandler : IRequestHandler<Post>
{
    private readonly IMediator _mediatr;

    public PostHandler(IMediator mediatr)
    {
        _mediatr = mediatr;
    }    

    public async Task Handle(Post request, CancellationToken cancellationToken)
    {
        var userId = await GetUserId(request.UserName, cancellationToken);
        var projectId = await GetProjectId(request.ProjectName, cancellationToken);

        var _ = await _mediatr.Send(new CreatePost(userId, projectId, request.Message), cancellationToken);
    }

    private async Task<UserId> GetUserId(string userName, CancellationToken cancellationToken)
    {
        var userIdResponse = await _mediatr.Send(new GetUserIdByName(userName), cancellationToken);
        if (userIdResponse.UserId.HasValue)
            return userIdResponse.UserId.Value;
        
        var createUserResponse = await _mediatr.Send(new CreateUser(userName), cancellationToken);
        return createUserResponse.UserId;
    }

    private async Task<ProjectId> GetProjectId(string projectName, CancellationToken cancellationToken)
    {
        var projectIdResponse = await _mediatr.Send(new GetProjectIdByName(projectName), cancellationToken);
        if (projectIdResponse.ProjectId.HasValue)
            return projectIdResponse.ProjectId.Value;
        
        var createProjectResponse = await _mediatr.Send(new CreateProject(projectName), cancellationToken);
        return createProjectResponse.ProjectId;
    }
}
