using Microsoft.EntityFrameworkCore;
using Quiz.Infrastructure.Persistence;
using Quiz.Infrastructure.Repositories;
using Quiz.Core.Services;
using Quiz.Core.Repositories;
using Quiz.Core.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<QuizDbContext>(options =>
    options.UseSqlServer(
        "Server=(localdb)\\mssqllocaldb;Database=QuizDb;Trusted_Connection=True;"
    ));

builder.Services.AddScoped<IRepository<QuizModel>, EfQuizRepository>();
builder.Services.AddScoped<QuizService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();