using JeremySkippen.MessageBoard.DomainModel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JeremySkippen.MessageBoard.Projects;

public sealed record GetProjectTimeline(ProjectId ProjectId) : IRequest<GetProjectTimelineResponse>;

public sealed class GetProjectTimelineHandler : IRequestHandler<GetProjectTimeline, GetProjectTimelineResponse>
{
    private readonly MessageBoardContext _dbContext;

    public GetProjectTimelineHandler(MessageBoardContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetProjectTimelineResponse> Handle(GetProjectTimeline request, CancellationToken cancellationToken)
    {
        var items = await _dbContext.Postings!
            .Where(i => i.ProjectId == request.ProjectId)
            .Select(i => new
            {
                i.PostedBy!.UserName,
                i.Message,
                i.PostingDateTime
            })
            .ToListAsync(cancellationToken);

        return new(
            items
                .OrderBy(i => i.PostingDateTime)
                .Select(i => new GetProjectTimelineResponseItem(i.UserName, i.Message, i.PostingDateTime))
                .ToList()
        );
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
