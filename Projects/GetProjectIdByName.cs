using JeremySkippen.MessageBoard.DomainModel;
using MediatR;

namespace JeremySkippen.MessageBoard.Projects;

public sealed record GetProjectIdByName(string ProjectName) : IRequest<GetProjectIdByNameResponse>;

public sealed class GetProjectIdByNameHandler : IRequestHandler<GetProjectIdByName, GetProjectIdByNameResponse>
{
    private readonly DataStore _dbContext;

    public GetProjectIdByNameHandler(DataStore dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<GetProjectIdByNameResponse> Handle(GetProjectIdByName request, CancellationToken cancellationToken)
    {
        var project = _dbContext.FindProjectByName(request.ProjectName);

        return Task.FromResult(new GetProjectIdByNameResponse(project?.ProjectId));
    }
}

public sealed record GetProjectIdByNameResponse(ProjectId? ProjectId);
