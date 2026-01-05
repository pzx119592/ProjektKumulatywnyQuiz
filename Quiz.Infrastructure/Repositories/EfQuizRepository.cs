using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quiz.Core.Models;
using Quiz.Core.Repositories;
using Quiz.Infrastructure.Entities;
using Quiz.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Infrastructure.Repositories;

public class EfQuizRepository : IRepository<QuizModel>
{
    private readonly QuizDbContext _context;

    public EfQuizRepository(QuizDbContext context)
    {
        _context = context;
    }

    public void Add(QuizModel quiz)
    {
        _context.Quizzes.Add(MapToEntity(quiz));
        _context.SaveChanges();
    }

    public void Remove(QuizModel quiz)
    {
        var entity = _context.Quizzes.Find(quiz.Id);
        if (entity == null) return;

        _context.Quizzes.Remove(entity);
        _context.SaveChanges();
    }

    public void Update(QuizModel quiz)
    {
        var entity = _context.Quizzes.Find(quiz.Id);
        if (entity == null) return;

        entity.Title = quiz.Title;
        _context.SaveChanges();
    }

    public IReadOnlyList<QuizModel> GetAll()
    {
        return _context.Quizzes
            .Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .AsNoTracking()
            .Select(MapToModel)
            .ToList();
    }

    // 🔹 ASYNC — TO JEST KLUCZ DO ZALICZENIA
    public async Task<IReadOnlyList<QuizModel>> GetAllAsync()
    {
        var entities = await _context.Quizzes
            .Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToModel).ToList();
    }

    public async Task<QuizModel?> GetByIdAsync(int id)
    {
        var entity = await _context.Quizzes
            .Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id);

        if (entity == null)
            return null;

        return MapToModel(entity);
    }


    private static QuizEntity MapToEntity(QuizModel quiz)
    {
        return new QuizEntity
        {
            Title = quiz.Title,
            Questions = quiz.Questions.Select(q => new QuestionEntity
            {
                Content = q.Content,
                Answers = q.Answers.Select(a => new AnswerEntity
                {
                    Content = a.Content,
                    IsCorrect = a.IsCorrect
                }).ToList()
            }).ToList()
        };
    }

    private static QuizModel MapToModel(QuizEntity entity)
    {
        var quiz = new QuizModel(entity.Id, entity.Title);

        foreach (var q in entity.Questions)
        {
            var question = new Question(q.Id, q.Content);

            foreach (var a in q.Answers)
                question.AddAnswer(new Answer(a.Id, a.Content, a.IsCorrect));

            quiz.AddQuestion(question);
        }

        return quiz;
    }
}