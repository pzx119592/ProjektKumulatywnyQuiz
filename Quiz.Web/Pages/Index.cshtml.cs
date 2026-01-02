using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quiz.Core.Models;
using Quiz.Core.Services;

public class IndexModel : PageModel
{
    private readonly QuizService _quizService;

    public IReadOnlyList<QuizModel> Quizzes { get; private set; } = new List<QuizModel>();

    public IndexModel(QuizService quizService)
    {
        _quizService = quizService;
    }

    public void OnGet()
    {
        Quizzes = _quizService.GetAllQuizzes();
    }
}
