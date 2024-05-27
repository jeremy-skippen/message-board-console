using JeremySkippen.MessageBoard.DomainModel;
using MediatR;

namespace JeremySkippen.MessageBoard.Users;

public sealed record CreateUser(string UserName) : IRequest<CreateUserResponse>;

public sealed class CreateUserHandler : IRequestHandler<CreateUser, CreateUserResponse>
{
    private readonly DataStore _dbContext;

    public CreateUserHandler(DataStore dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CreateUserResponse> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        var user = new User(UserId.NewId(), request.UserName);
        
        _dbContext.AddUser(user);

        return Task.FromResult(new CreateUserResponse(user.UserId));
    }
}

public sealed record CreateUserResponse(UserId UserId);
