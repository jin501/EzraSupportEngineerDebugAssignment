using Microsoft.EntityFrameworkCore;
using SupportEngineerChallenge.Api.Models;

namespace SupportEngineerChallenge.Api.Data;

public static class DbSeeder
{
    public static async Task SeedIfNeededAsync(AppDbContext db, IConfiguration config)
    {
        var enabled = config.GetValue<bool>("Seed:Enabled");
        if (!enabled) return;

        if (await db.Tasks.AnyAsync()) return;

        var users = Math.Max(1, config.GetValue<int>("Seed:Users"));
        var tasksPerUser = Math.Max(1, config.GetValue<int>("Seed:TasksPerUser"));

        var now = DateTime.UtcNow;
        var rnd = new Random(1337);

        var batch = new List<TaskItem>(capacity: 1000);

        for (var u = 1; u <= users; u++)
        {
            var userId = $"user-{u:000}";
            for (var i = 0; i < tasksPerUser; i++)
            {
                var createdAt = now.AddMinutes(-1 * rnd.Next(0, 60 * 24 * 14)); // within last 2 weeks
                batch.Add(new TaskItem
                {
                    UserId = userId,
                    Title = $"Seeded task {i + 1} for {userId}",
                    Status = (i % 5 == 0) ? "done" : "open",
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                });

                if (batch.Count >= 1000)
                {
                    db.Tasks.AddRange(batch);
                    await db.SaveChangesAsync();
                    batch.Clear();
                }
            }
        }

        if (batch.Count > 0)
        {
            db.Tasks.AddRange(batch);
            await db.SaveChangesAsync();
        }
    }
}
