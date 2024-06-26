using JeremySkippen.MessageBoard.DomainModel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JeremySkippen.MessageBoard.Projects;

public sealed record GetProjectIdByName(string ProjectName) : IRequest<GetProjectIdByNameResponse>;

public sealed class GetProjectIdByNameHandler : IRequestHandler<GetProjectIdByName, GetProjectIdByNameResponse>
{
    private readonly MessageBoardContext _dbContext;

    public GetProjectIdByNameHandler(MessageBoardContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetProjectIdByNameResponse> Handle(GetProjectIdByName request, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects!.Where(u => u.ProjectName == request.ProjectName).FirstOrDefaultAsync(cancellationToken);
        if (project is null)
            return new(null);
        
        return new(project.ProjectId);
    }
}

public sealed record GetProjectIdByNameResponse(ProjectId? ProjectId);
