using System.Text.RegularExpressions;
using MediatR;

namespace JeremySkippen.MessageBoard.Commands;

public sealed record InterpretCommandLine(string CommandLine) : IRequest<string>;

public sealed class InterpretCommandLineHandler : IRequestHandler<InterpretCommandLine, string>
{
    private readonly IMediator _mediatr;

    private static readonly Regex PostingRegex = new(@"^(\w+) -> @(\w+) (.+)$", RegexOptions.Compiled);
    private static readonly Regex ReadingRegex = new(@"^(\w+)$", RegexOptions.Compiled);
    private static readonly Regex FollowingRegex = new(@"^(\w+) follows (\w+)$", RegexOptions.Compiled);
    private static readonly Regex WallRegex = new(@"^(\w+) wall$", RegexOptions.Compiled);

    public InterpretCommandLineHandler(IMediator mediatr)
    {
        _mediatr = mediatr;
    }    

    public async Task<string> Handle(InterpretCommandLine request, CancellationToken cancellationToken)
    {
        var postingMatch = PostingRegex.Match(request.CommandLine);
        if (postingMatch.Success)
        {
            await _mediatr.Send(new Post(
                postingMatch.Groups[1].Value,
                postingMatch.Groups[2].Value,
                postingMatch.Groups[3].Value
            ), cancellationToken);
            return "";
        }

        var readingMatch = ReadingRegex.Match(request.CommandLine);
        if (readingMatch.Success)
        {
            return await _mediatr.Send(new Read(readingMatch.Groups[1].Value), cancellationToken);
        }

        return "";
    }
}
