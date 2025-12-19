using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProjektKumulatywnyQuiz
{
    public interface IQuiz
    {
        int Id { get; set; }
        string Title { get; set; }
        List<Question> Questions { get; set; }
    }
}