using JeremySkippen.MessageBoard.DomainModel;
using MediatR;

namespace JeremySkippen.MessageBoard.Users;

public sealed record CreateUser(string UserName) : IRequest<CreateUserResponse>;

public sealed class CreateUserHandler : IRequestHandler<CreateUser, CreateUserResponse>
{
    private readonly MessageBoardContext _dbContext;

    public CreateUserHandler(MessageBoardContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateUserResponse> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        var user = new User(UserId.NewId(), request.UserName);
        
        _dbContext.Users?.Add(user);
        await _dbContext.SaveChangesAsync();

        return new(user.UserId);
    }
}

public sealed record CreateUserResponse(UserId UserId);
