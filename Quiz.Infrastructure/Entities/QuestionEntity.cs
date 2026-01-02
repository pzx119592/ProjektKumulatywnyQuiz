using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Infrastructure.Entities;

public class QuestionEntity
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;

    public int QuizEntityId { get; set; }
    public QuizEntity Quiz { get; set; } = null!;

    public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
}
