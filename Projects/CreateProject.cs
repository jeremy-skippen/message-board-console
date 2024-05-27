using JeremySkippen.MessageBoard.DomainModel;
using MediatR;

namespace JeremySkippen.MessageBoard.Projects;

public sealed record CreateProject(string ProjectName) : IRequest<CreateProjectResponse>;

public sealed class CreateProjectHandler : IRequestHandler<CreateProject, CreateProjectResponse>
{
    private readonly DataStore _dbContext;

    public CreateProjectHandler(DataStore dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CreateProjectResponse> Handle(CreateProject request, CancellationToken cancellationToken)
    {
        var project = new Project(ProjectId.NewId(), request.ProjectName);
        
        _dbContext.AddProject(project);

        return Task.FromResult(new CreateProjectResponse(project.ProjectId));
    }
}

public sealed record CreateProjectResponse(ProjectId ProjectId);
