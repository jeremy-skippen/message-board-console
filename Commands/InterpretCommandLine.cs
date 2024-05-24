using MediatR;

namespace JeremySkippen.MessageBoard.Commands;

public sealed record InterpretCommandLine(string CommandLine) : IRequest;

public sealed class InterpretCommandLineHandler : IRequestHandler<InterpretCommandLine>
{
    private readonly IMediator _mediatr;

    public InterpretCommandLineHandler(IMediator mediatr)
    {
        _mediatr = mediatr;
    }    

    public Task Handle(InterpretCommandLine request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"TODO: interpret ~ {request.CommandLine}");

        return Task.CompletedTask;
    }
}
