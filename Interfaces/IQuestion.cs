using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProjektKumulatywnyQuiz
{
    public interface IQuestion
    {
        int Id { get; set; }
        string Text { get; set; }
        List<Answer> Answers { get; set; }
    }
}