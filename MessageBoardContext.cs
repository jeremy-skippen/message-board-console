using JeremySkippen.MessageBoard.DomainModel;

using Microsoft.EntityFrameworkCore;

namespace JeremySkippen.MessageBoard;

public sealed class MessageBoardContext : DbContext
{
    public DbSet<Posting>? Postings { get; set; }
    public DbSet<Project>? Projects { get; set; }
    public DbSet<User>? Users { get; set; }

    public MessageBoardContext(
        DbContextOptions<MessageBoardContext> dbContextOptions
    ) : base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Posting>(entity =>
        {
            entity.HasKey(p => p.PostingId);

            entity.Property(p => p.PostingId).HasConversion(postingId => postingId.Id, value => new PostingId(value));
            entity.Property(p => p.PostedById).HasConversion(postedById => postedById.Id, value => new UserId(value));
            entity.Property(p => p.ProjectId).HasConversion(projectId => projectId.Id, value => new ProjectId(value));

            entity
                .HasOne(p => p.PostedBy)
                .WithMany(u => u.Postings)
                .HasForeignKey(p => p.PostedById);
            entity
                .HasOne(p => p.Project)
                .WithMany(u => u.Postings)
                .HasForeignKey(p => p.ProjectId);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.ProjectId);

            entity.Property(p => p.ProjectId).HasConversion(projectId => projectId.Id, value => new ProjectId(value));
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);

            entity.Property(p => p.UserId).HasConversion(userId => userId.Id, value => new UserId(value));

            entity
                .HasMany(u => u.FollowingProjects)
                .WithMany(p => p.FollowedByUsers)
                .UsingEntity<UserFollow>();
        });

        modelBuilder.Entity<UserFollow>(entity =>
        {
            entity.HasKey(uf => new { uf.UserId, uf.ProjectId });

            entity.Property(p => p.UserId).HasConversion(userId => userId.Id, value => new UserId(value));
            entity.Property(p => p.ProjectId).HasConversion(projectId => projectId.Id, value => new ProjectId(value));
        });
    }
}
