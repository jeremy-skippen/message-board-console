using JeremySkippen.MessageBoard.DomainModel;
using MediatR;

namespace JeremySkippen.MessageBoard.Projects;

public sealed record GetProjectTimeline(ProjectId ProjectId) : IRequest<GetProjectTimelineResponse>;

public sealed class GetProjectTimelineHandler : IRequestHandler<GetProjectTimeline, GetProjectTimelineResponse>
{
    private readonly DataStore _dbContext;

    public GetProjectTimelineHandler(DataStore dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<GetProjectTimelineResponse> Handle(GetProjectTimeline request, CancellationToken cancellationToken)
    {
        var items = _dbContext.GetPostingsByProjectId(request.ProjectId)
            .Select(i => new GetProjectTimelineResponseItem(
                i.PostedBy?.UserName ?? "",
                i.Message,
                i.PostingDateTime
            ))
            .OrderBy(i => i.PostDateTime)
            .ToList();

        return Task.FromResult(new GetProjectTimelineResponse(items));
    }
}

public sealed record GetProjectTimelineResponse(
    IReadOnlyList<GetProjectTimelineResponseItem> Items
);

public sealed record GetProjectTimelineResponseItem(
    string UserName,
    string Message,
    DateTimeOffset PostDateTime
);
