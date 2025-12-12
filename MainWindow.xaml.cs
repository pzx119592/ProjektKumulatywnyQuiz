using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace ProjektKumulatywnyQuiz
{
    public partial class MainWindow : Window
    {
        private readonly QuizDbContext _db;
        private Quiz? _currentQuiz;

        public MainWindow()
        {
            InitializeComponent();
            _db = new QuizDbContext();
            _db.Database.Migrate();

            AddQuizBtn.Click += AddQuizBtn_Click;
            LoadBtn.Click += LoadBtn_Click;
            SearchBtn.Click += SearchBtn_Click;
            CheckBtn.Click += CheckBtn_Click;
            UpdateBtn.Click += UpdateBtn_Click;
            DeleteBtn.Click += DeleteBtn_Click;
        }

        // CREATE
        private void AddQuizBtn_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Podaj tytuł quizu");
                return;
            }

            var quiz = new Quiz
            {
                Title = title,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "Stolica Francji?",
                        Answers = new List<Answer>
                        {
                            new Answer { Text = "Paryż", IsCorrect = true },
                            new Answer { Text = "Lyon", IsCorrect = false }
                        }
                    }
                }
            };

            _db.Quizzes.Add(quiz);
            _db.SaveChanges();
            LoadQuizzes();
        }

        // READ
        private void LoadBtn_Click(object sender, RoutedEventArgs e) => LoadQuizzes();

        private void LoadQuizzes()
        {
            var quizzes = _db.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .ToList();

            QuizList.ItemsSource = quizzes.Select(q => $"{q.Id}: {q.Title} ({q.Questions.Count} pytań)").ToList();
        }

        // SEARCH (LINQ)
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var term = TitleBox.Text.Trim();
            var query = _db.Quizzes.AsQueryable();

            if (!string.IsNullOrEmpty(term))
                query = query.Where(q => EF.Functions.Like(q.Title, $"%{term}%"));

            var result = query.ToList();
            QuizList.ItemsSource = result.Select(q => $"{q.Id}: {q.Title}").ToList();
        }

        // READ (szczegóły quizu)
        private void QuizList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (QuizList.SelectedItem == null) return;

            var selectedText = QuizList.SelectedItem.ToString();
            var id = int.Parse(selectedText.Split(':')[0]);

            _currentQuiz = _db.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .FirstOrDefault(q => q.Id == id);

            if (_currentQuiz != null)
            {
                QuizTitle.Text = _currentQuiz.Title;
                QuestionsList.ItemsSource = _currentQuiz.Questions;
            }
        }

        // UPDATE
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (QuizList.SelectedItem == null)
            {
                MessageBox.Show("Wybierz quiz do edycji");
                return;
            }

            var selected = QuizList.SelectedItem.ToString();
            var id = int.Parse(selected.Split(':')[0]);

            var quiz = _db.Quizzes.FirstOrDefault(q => q.Id == id);
            if (quiz == null)
            {
                MessageBox.Show("Nie znaleziono quizu");
                return;
            }

            var newTitle = TitleBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                MessageBox.Show("Podaj nowy tytuł");
                return;
            }

            quiz.Title = newTitle;
            _db.SaveChanges();
            LoadQuizzes();
            MessageBox.Show("Quiz został zaktualizowany");
        }

        // DELETE
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (QuizList.SelectedItem == null)
            {
                MessageBox.Show("Wybierz quiz do usunięcia");
                return;
            }

            var selected = QuizList.SelectedItem.ToString();
            var id = int.Parse(selected.Split(':')[0]);

            var quiz = _db.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .FirstOrDefault(q => q.Id == id);

            if (quiz == null)
            {
                MessageBox.Show("Nie znaleziono quizu");
                return;
            }

            _db.Quizzes.Remove(quiz);
            _db.SaveChanges();
            LoadQuizzes();
            MessageBox.Show("Quiz został usunięty");
        }

        // QUIZ LOGIC
        private void CheckBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuiz == null) return;

            int correct = 0;
            int total = _currentQuiz.Questions.Count;

            foreach (var question in _currentQuiz.Questions)
            {
                var container = QuestionsList.ItemContainerGenerator.ContainerFromItem(question) as FrameworkElement;
                if (container != null)
                {
                    var radios = container.FindVisualChildren<System.Windows.Controls.RadioButton>();
                    var selected = radios.FirstOrDefault(r => r.IsChecked == true);

                    if (selected != null && (bool)selected.Tag == true)
                        correct++;
                }
            }

            MessageBox.Show($"Twój wynik: {correct}/{total}");
        }
    }

    public static class VisualTreeHelperExtensions
    {
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);

                    if (child is T t)
                        yield return t;

                    foreach (var childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
    }
}