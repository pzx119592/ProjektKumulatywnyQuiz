using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

// --- Zadanie 5: Entity Framework Core ---
// Klasa QuizDbContext odpowiada za połączenie z bazą danych.
// DbContext to centralny element EF Core — pozwala mapować klasy C# na tabele w bazie.
public class QuizDbContext : DbContext
{
    // DbSet<Quiz> reprezentuje tabelę Quizów w bazie danych.
    public DbSet<Quiz> Quizzes => Set<Quiz>();

    // DbSet<Question> reprezentuje tabelę Pytań.
    public DbSet<Question> Questions => Set<Question>();

    // DbSet<Answer> reprezentuje tabelę Odpowiedzi.
    public DbSet<Answer> Answers => Set<Answer>();

    // OnConfiguring ustawia sposób połączenia z bazą danych.
    // W tym przypadku używamy SQLite i zapisujemy dane w pliku quiz.db.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=quiz.db");
    }
}