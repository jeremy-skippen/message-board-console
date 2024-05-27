using JeremySkippen.MessageBoard.DomainModel;
using MediatR;

namespace JeremySkippen.MessageBoard.Users;

public sealed record GetUserIdByName(string UserName) : IRequest<GetUserIdByNameResponse>;

public sealed class GetUserIdByNameHandler : IRequestHandler<GetUserIdByName, GetUserIdByNameResponse>
{
    private readonly DataStore _dbContext;

    public GetUserIdByNameHandler(DataStore dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<GetUserIdByNameResponse> Handle(GetUserIdByName request, CancellationToken cancellationToken)
    {
        var user = _dbContext.FindUserByName(request.UserName);

        return Task.FromResult(new GetUserIdByNameResponse(user?.UserId));
    }
}

public sealed record GetUserIdByNameResponse(UserId? UserId);
