using System.Text;
using JeremySkippen.MessageBoard.DomainModel;
using JeremySkippen.MessageBoard.Projects;
using MediatR;

namespace JeremySkippen.MessageBoard.Commands;

public sealed record Read(string ProjectName) : IRequest<string>;

public sealed class ReadHandler : IRequestHandler<Read, string>
{
    private readonly IMediator _mediatr;

    public ReadHandler(IMediator mediatr)
    {
        _mediatr = mediatr;
    }    

    public async Task<string> Handle(Read request, CancellationToken cancellationToken)
    {
        var projectId = await GetProjectId(request.ProjectName, cancellationToken);

        var timeline = await _mediatr.Send(new GetProjectTimeline(projectId), cancellationToken);

        StringBuilder timelineBuild = new();
        string? previousUsername = null;
        foreach (var item in timeline.Items)
        {
            if (item.UserName != previousUsername)
                timelineBuild.AppendLine(item.UserName);

            previousUsername = item.UserName;
            timelineBuild.AppendLine($"{item.Message} ({item.PostDateTime})");
        }

        return timelineBuild.ToString();
    }

    private async Task<ProjectId> GetProjectId(string projectName, CancellationToken cancellationToken)
    {
        var projectIdResponse = await _mediatr.Send(new GetProjectIdByName(projectName));
        if (projectIdResponse.ProjectId.HasValue)
            return projectIdResponse.ProjectId.Value;
        
        var createProjectResponse = await _mediatr.Send(new CreateProject(projectName));
        return createProjectResponse.ProjectId;
    }
}
