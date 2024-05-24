using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JeremySkippen.MessageBoard;

public class MessageBoardContextDesignTimeFactory : IDesignTimeDbContextFactory<MessageBoardContext>
{
    public MessageBoardContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<MessageBoardContext>()
            .UseSqlite("Filename=messageboard.db")
            .Options;

        return new MessageBoardContext(options);
    }
}
