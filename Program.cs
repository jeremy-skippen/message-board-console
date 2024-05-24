using JeremySkippen.MessageBoard;
using JeremySkippen.MessageBoard.Commands;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddDbContext<MessageBoardContext>(cfg =>
    {
        cfg.UseSqlite("Filename=messageboard.db");
    })
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    })
    .BuildServiceProvider();

using var scope = serviceProvider.CreateScope();

var context = serviceProvider.GetRequiredService<MessageBoardContext>();
context.Database.EnsureCreated();

var mediatr = serviceProvider.GetRequiredService<IMediator>();
for (; ; )
{
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line is not null)
        await mediatr.Send(new InterpretCommandLine(line));
}
