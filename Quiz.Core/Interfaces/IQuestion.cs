using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Core.Interfaces;

public interface IQuestion
{
    int Id { get; }
    string Content { get; }
    IReadOnlyList<IAnswer> Answers { get; }
}