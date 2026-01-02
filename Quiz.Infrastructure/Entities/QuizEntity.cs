using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Infrastructure.Entities;

public class QuizEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
}
