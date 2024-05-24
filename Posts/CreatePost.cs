using JeremySkippen.MessageBoard.DomainModel;
using MediatR;

namespace JeremySkippen.MessageBoard.Posts;

public sealed record CreatePost(
    UserId PostedById,
    ProjectId ProjectId,
    string Message
) : IRequest<CreatePostResponse>;

public sealed class CreatePostHandler : IRequestHandler<CreatePost, CreatePostResponse>
{
    private readonly MessageBoardContext _dbContext;

    public CreatePostHandler(MessageBoardContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreatePostResponse> Handle(CreatePost request, CancellationToken cancellationToken)
    {
        var Posting = new Posting(
            PostingId.NewId(),
            request.PostedById,
            request.ProjectId,
            request.Message,
            DateTimeOffset.Now
        );
        
        _dbContext.Postings?.Add(Posting);
        await _dbContext.SaveChangesAsync();

        return new(Posting.PostingId);
    }
}

public sealed record CreatePostResponse(PostingId PostingId);
