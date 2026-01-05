using Microsoft.EntityFrameworkCore;
using Quiz.Core.Models;
using Quiz.Core.Services;
using Quiz.Infrastructure.Persistence;
using Quiz.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Quiz.WpfApp;

public partial class MainWindow : Window
{
    private readonly QuizService _quizService;
    private List<QuizModel> _quizzes = new();
    private QuizModel? _currentQuiz;
    private Question? _currentQuestion;

    public MainWindow()
    {
        InitializeComponent();

        var options = new DbContextOptionsBuilder<QuizDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=QuizDb;Trusted_Connection=True;")
            .Options;

        _quizService = new QuizService(
            new EfQuizRepository(new QuizDbContext(options))
        );

        _ = LoadQuizzesAsync();
    }

    // 🔹 ASYNC WCZYTYWANIE Z BAZY (ZEWNĘTRZNE ŹRÓDŁO)
    private async Task LoadQuizzesAsync()
    {
        QuizList.ItemsSource = null;

        // opcjonalne – pokazuje, że UI się nie blokuje
        await Task.Delay(1000);

        _quizzes = (await _quizService.GetAllQuizzesAsync()).ToList();
        QuizList.ItemsSource = _quizzes;
    }

    private void AddQuiz_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(QuizTitleBox.Text)) return;

        _currentQuiz = new QuizModel(0, QuizTitleBox.Text);
        _quizzes.Add(_currentQuiz);
        RefreshQuizList();
        QuizTitleBox.Clear();
    }

    private void RenameQuiz_Click(object sender, RoutedEventArgs e)
    {
        if (_currentQuiz == null || string.IsNullOrWhiteSpace(QuizTitleBox.Text)) return;

        _currentQuiz.ChangeTitle(QuizTitleBox.Text);
        _quizService.UpdateQuiz(_currentQuiz);
        _ = LoadQuizzesAsync();
    }

    private void DeleteQuiz_Click(object sender, RoutedEventArgs e)
    {
        if (_currentQuiz == null) return;

        _quizService.RemoveQuiz(_currentQuiz);
        _ = LoadQuizzesAsync();
        _currentQuiz = null;
    }

    private void SaveQuiz_Click(object sender, RoutedEventArgs e)
    {
        if (_currentQuiz == null || _currentQuiz.Questions.Count == 0) return;

        _quizService.AddQuiz(_currentQuiz);
        _ = LoadQuizzesAsync();
    }

    private void AddQuestion_Click(object sender, RoutedEventArgs e)
    {
        if (_currentQuiz == null || string.IsNullOrWhiteSpace(QuestionBox.Text)) return;

        var q = new Question(0, QuestionBox.Text);
        _currentQuiz.AddQuestion(q);
        QuestionList.ItemsSource = _currentQuiz.Questions;
        QuestionBox.Clear();
    }

    private void AddAnswer_Click(object sender, RoutedEventArgs e)
    {
        if (_currentQuestion == null || string.IsNullOrWhiteSpace(AnswerBox.Text)) return;

        _currentQuestion.AddAnswer(
            new Answer(0, AnswerBox.Text, IsCorrectCheckBox.IsChecked == true)
        );

        AnswerList.ItemsSource = _currentQuestion.Answers;
        AnswerBox.Clear();
    }

    private async void QuizList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (QuizList.SelectedItem is not QuizModel selected)
            return;

        // krótki delay tylko demonstracyjnie
        await Task.Delay(500);

        _currentQuiz = await _quizService.GetQuizByIdAsync(selected.Id);
        QuestionList.ItemsSource = _currentQuiz?.Questions;
    }


    private void QuestionList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        _currentQuestion = QuestionList.SelectedItem as Question;
        AnswerList.ItemsSource = _currentQuestion?.Answers;
    }

    private void FilterBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        var text = FilterBox.Text.ToLower();
        QuizList.ItemsSource = _quizzes
            .Where(q => q.Title.ToLower().Contains(text))
            .ToList();
    }

    private void RefreshQuizList()
    {
        QuizList.ItemsSource = null;
        QuizList.ItemsSource = _quizzes;
    }
}