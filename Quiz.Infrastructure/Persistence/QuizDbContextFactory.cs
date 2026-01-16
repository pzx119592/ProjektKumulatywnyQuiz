using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Quiz.Infrastructure.Persistence;

// Klasa pomocnicza do EF, łączenie się z bazą danych przy migracjach
public class QuizDbContextFactory : IDesignTimeDbContextFactory<QuizDbContext>
{
    public QuizDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<QuizDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=QuizDb;Trusted_Connection=True;"
        );

        return new QuizDbContext(optionsBuilder.Options);
    }
}