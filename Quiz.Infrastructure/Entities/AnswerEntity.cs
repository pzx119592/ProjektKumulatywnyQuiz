using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Infrastructure.Entities;

public class AnswerEntity
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }

    public int QuestionEntityId { get; set; }
    public QuestionEntity Question { get; set; } = null!;
}