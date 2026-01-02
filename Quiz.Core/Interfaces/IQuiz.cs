using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Core.Interfaces;

public interface IQuiz
{
    int Id { get; }
    string Title { get; }
    IReadOnlyList<IQuestion> Questions { get; }
}