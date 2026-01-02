using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.Core.Interfaces;

namespace Quiz.Core.Models;

public class Question : IQuestion
{
    private readonly List<IAnswer> _answers = new();

    public int Id { get; private set; }
    public string Content { get; private set; }
    public IReadOnlyList<IAnswer> Answers => _answers;

    public Question(int id, string content)
    {
        Id = id;
        Content = content;
    }

    public void AddAnswer(IAnswer answer)
    {
        _answers.Add(answer);
    }
}