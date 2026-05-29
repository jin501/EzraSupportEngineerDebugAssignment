using Microsoft.EntityFrameworkCore;
using SupportEngineerChallenge.Api.Models;

namespace SupportEngineerChallenge.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.Title).IsRequired();
            b.Property(x => x.Status).IsRequired();
        });
    }
}
