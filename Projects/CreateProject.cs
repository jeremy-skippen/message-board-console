using JeremySkippen.MessageBoard.DomainModel;
using MediatR;

namespace JeremySkippen.MessageBoard.Projects;

public sealed record CreateProject(string ProjectName) : IRequest<CreateProjectResponse>;

public sealed class CreateProjectHandler : IRequestHandler<CreateProject, CreateProjectResponse>
{
    private readonly MessageBoardContext _dbContext;

    public CreateProjectHandler(MessageBoardContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateProjectResponse> Handle(CreateProject request, CancellationToken cancellationToken)
    {
        var Project = new Project(ProjectId.NewId(), request.ProjectName);
        
        _dbContext.Projects?.Add(Project);
        await _dbContext.SaveChangesAsync();

        return new(Project.ProjectId);
    }
}

public sealed record CreateProjectResponse(ProjectId ProjectId);
