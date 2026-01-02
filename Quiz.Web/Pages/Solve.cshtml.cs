using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quiz.Core.Models;
using Quiz.Core.Services;

public class SolveModel : PageModel
{
    private readonly QuizService _quizService;

    public QuizModel Quiz { get; private set; } = null!;
    public int Score { get; private set; }

    [BindProperty]
    public Dictionary<int, int> Answers { get; set; } = new();

    public SolveModel(QuizService quizService)
    {
        _quizService = quizService;
    }

    public IActionResult OnGet(int id)
    {
        var quiz = _quizService.GetAllQuizzes().FirstOrDefault(q => q.Id == id);
        if (quiz == null)
            return RedirectToPage("/Index");

        Quiz = quiz;
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        Quiz = _quizService.GetAllQuizzes().First(q => q.Id == id);

        foreach (var question in Quiz.Questions)
        {
            if (Answers.TryGetValue(question.Id, out var answerId))
            {
                if (question.Answers.First(a => a.Id == answerId).IsCorrect)
                    Score++;
            }
        }

        return Page();
    }
}
