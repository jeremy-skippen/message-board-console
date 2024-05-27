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
    private readonly DataStore _dbContext;

    public CreatePostHandler(DataStore dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CreatePostResponse> Handle(CreatePost request, CancellationToken cancellationToken)
    {
        var posting = new Posting(
            PostingId.NewId(),
            request.PostedById,
            request.ProjectId,
            request.Message,
            DateTimeOffset.Now
        );
        
        _dbContext.AddPosting(posting);

        return Task.FromResult(new CreatePostResponse(posting.PostingId));
    }
}

public sealed record CreatePostResponse(PostingId PostingId);
