using Microsoft.EntityFrameworkCore;
using Quiz.Core.Models;
using Quiz.Core.Services;
using Quiz.Infrastructure.Persistence;
using Quiz.Infrastructure.Repositories;

var options = new DbContextOptionsBuilder<QuizDbContext>()
    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=QuizDb;Trusted_Connection=True;")
    .Options;

using var context = new QuizDbContext(options);

var quizRepository = new EfQuizRepository(context);
var quizService = new QuizService(quizRepository);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Dodaj quiz");
    Console.WriteLine("2. Wyświetl quizy");
    Console.WriteLine("0. Wyjście");

    Console.Write("Wybór: ");
    var choice = Console.ReadLine();

    if (choice == "0")
        break;

    if (choice == "1")
    {
        Console.Write("Podaj tytuł quizu: ");
        var title = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title))
            continue;

        var quiz = new QuizModel(0, title);

        while (true)
        {
            Console.Write("Treść pytania (ENTER kończy): ");
            var questionText = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(questionText))
                break;

            var question = new Question(0, questionText);

            while (true)
            {
                Console.Write("Odpowiedź (ENTER kończy): ");
                var answerText = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(answerText))
                    break;

                Console.Write("Czy poprawna? (t/n): ");
                var isCorrect = Console.ReadLine()?.ToLower() == "t";

                var answer = new Answer(0, answerText, isCorrect);
                question.AddAnswer(answer);
            }

            quiz.AddQuestion(question);
        }

        quizService.AddQuiz(quiz);
        Console.WriteLine("Quiz zapisany w bazie danych.");
    }

    if (choice == "2")
    {
        var quizzes = quizService.GetAllQuizzes();

        foreach (var q in quizzes)
        {
            Console.WriteLine();
            Console.WriteLine($"Quiz {q.Id}: {q.Title}");

            foreach (var question in q.Questions)
            {
                Console.WriteLine($"  Pytanie: {question.Content}");

                foreach (var answer in question.Answers)
                {
                    var mark = answer.IsCorrect ? "[OK]" : "[ ]";
                    Console.WriteLine($"    {mark} {answer.Content}");
                }
            }
        }
    }
}