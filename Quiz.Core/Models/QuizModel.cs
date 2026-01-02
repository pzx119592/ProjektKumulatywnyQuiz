using Quiz.Core.Interfaces;
using System.Collections.Generic;

namespace Quiz.Core.Models;

public class QuizModel : IQuiz
{
    private readonly List<IQuestion> _questions = new();

    public int Id { get; private set; }
    public string Title { get; private set; }
    public IReadOnlyList<IQuestion> Questions => _questions;

    public QuizModel(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public void AddQuestion(IQuestion question)
    {
        _questions.Add(question);
    }

    public void ChangeTitle(string newTitle)
    {
        Title = newTitle;
    }
}
