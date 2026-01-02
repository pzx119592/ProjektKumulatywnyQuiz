using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.Core.Interfaces;

namespace Quiz.Core.Models;

public class Answer : IAnswer
{
    public int Id { get; private set; }
    public string Content { get; private set; }
    public bool IsCorrect { get; private set; }

    public Answer(int id, string content, bool isCorrect)
    {
        Id = id;
        Content = content;
        IsCorrect = isCorrect;
    }
}
