using JeremySkippen.MessageBoard.Commands;
using JeremySkippen.MessageBoard.DomainModel;
using MediatR;

using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddSingleton<DataStore>()
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    })
    .BuildServiceProvider();

using var scope = serviceProvider.CreateScope();

var mediatr = serviceProvider.GetRequiredService<IMediator>();
for ( ; ; )
{
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line is not null)
    {
        var response = await mediatr.Send(new InterpretCommandLine(line));
        if (!string.IsNullOrWhiteSpace(response))
        {
            Console.WriteLine(response);
        }
    }
}
