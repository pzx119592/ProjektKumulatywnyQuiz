using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.Core.Models;
using Quiz.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quiz.Core.Services;

public class QuizService
{
    private readonly IRepository<QuizModel> _quizRepository;

    public QuizService(IRepository<QuizModel> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public void AddQuiz(QuizModel quiz)
    {
        _quizRepository.Add(quiz);
    }

    public void RemoveQuiz(QuizModel quiz)
    {
        _quizRepository.Remove(quiz);
    }

    public void UpdateQuiz(QuizModel quiz)
    {
        _quizRepository.Update(quiz);
    }

    public IReadOnlyList<QuizModel> GetAllQuizzes()
    {
        return _quizRepository.GetAll();
    }

    //ASYNC — UŻYWANE W WPF
    public async Task<IReadOnlyList<QuizModel>> GetAllQuizzesAsync()
    {
        return await _quizRepository.GetAllAsync();
    }

    public async Task<QuizModel?> GetQuizByIdAsync(int id)
    {
        return await _quizRepository.GetByIdAsync(id);
    }
}