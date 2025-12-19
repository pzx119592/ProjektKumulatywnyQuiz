using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjektKumulatywnyQuiz
{
    public class Answer : IAnswer
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}