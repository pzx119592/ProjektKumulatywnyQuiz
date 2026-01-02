using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Core.Interfaces;

public interface IAnswer
{
    int Id { get; }
    string Content { get; }
    bool IsCorrect { get; }
}