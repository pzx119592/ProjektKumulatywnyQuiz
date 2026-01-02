using Microsoft.EntityFrameworkCore;
using Quiz.Infrastructure.Persistence;
using Quiz.Infrastructure.Repositories;
using Quiz.Core.Services;
using Quiz.Core.Repositories;
using Quiz.Core.Models;
using Quiz.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

// NOWY MODEL BLAZOR .NET 8
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// EF Core
builder.Services.AddDbContext<QuizDbContext>(options =>
    options.UseSqlServer(
        "Server=(localdb)\\mssqllocaldb;Database=QuizDb;Trusted_Connection=True;"
    ));

// DI
builder.Services.AddScoped<IRepository<QuizModel>, EfQuizRepository>();
builder.Services.AddScoped<QuizService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAntiforgery();

// START APLIKACJI BLAZOR
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();