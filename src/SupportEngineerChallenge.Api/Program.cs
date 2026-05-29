using Microsoft.EntityFrameworkCore;
using SupportEngineerChallenge.Api.Data;
using SupportEngineerChallenge.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("Default") ?? "Data Source=app.db";
    options.UseSqlite(cs);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

// --- Database init + seeding ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    await DbSeeder.SeedIfNeededAsync(db, builder.Configuration);
}

app.MapTaskEndpoints();

app.Run();

public partial class Program { } // for WebApplicationFactory
