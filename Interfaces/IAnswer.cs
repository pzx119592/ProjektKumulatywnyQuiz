using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektKumulatywnyQuiz
{
    public interface IAnswer
    {
        int Id { get; set; }
        string Text { get; set; }
        bool IsCorrect { get; set; }
    }
}