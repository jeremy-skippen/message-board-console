using JeremySkippen.MessageBoard.DomainModel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JeremySkippen.MessageBoard.Users;

public sealed record GetUserIdByName(string UserName) : IRequest<GetUserIdByNameResponse>;

public sealed class GetUserIdByNameHandler : IRequestHandler<GetUserIdByName, GetUserIdByNameResponse>
{
    private readonly MessageBoardContext _dbContext;

    public GetUserIdByNameHandler(MessageBoardContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetUserIdByNameResponse> Handle(GetUserIdByName request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users!.Where(u => u.UserName == request.UserName).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
            return new(null);
        
        return new(user.UserId);
    }
}

public sealed record GetUserIdByNameResponse(UserId? UserId);
