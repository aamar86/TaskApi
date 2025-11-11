using Microsoft.EntityFrameworkCore;
using Tasky.Domain.Entities;

namespace Tasky.Infrastructure.Persistence;

public class TaskyDbContext : DbContext
{
    public TaskyDbContext(DbContextOptions<TaskyDbContext> options) : base(options) { }
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}
