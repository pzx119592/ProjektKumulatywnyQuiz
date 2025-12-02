using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace ProjektKumulatywnyQuiz
{
    public partial class MainWindow : Window
    {
        private readonly QuizDbContext _db;   // --- Zadanie 5: EF Core - kontekst bazy danych
        private Quiz? _currentQuiz;           // Aktualnie wybrany quiz

        public MainWindow()
        {
            InitializeComponent();
            _db = new QuizDbContext();
            _db.Database.Migrate(); // --- Zadanie 5: EF Core - tworzenie bazy i tabel

            // --- Zadanie 4: WPF - podpięcie przycisków do metod ---
            AddQuizBtn.Click += AddQuizBtn_Click;
            LoadBtn.Click += LoadBtn_Click;
            SearchBtn.Click += SearchBtn_Click;
            CheckBtn.Click += CheckBtn_Click;
        }

        // --- Zadanie 5: EF Core CRUD - CREATE ---
        // Dodaje nowy quiz do bazy danych
        private void AddQuizBtn_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Podaj tytuł quizu");
                return;
            }

            // Tworzymy quiz z jednym przykładowym pytaniem
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

            _db.Quizzes.Add(quiz);   // dodanie quizu
            _db.SaveChanges();       // zapis do bazy
            LoadQuizzes();           // odświeżenie listy quizów
        }

        // --- Zadanie 5: EF Core CRUD - READ ---
        // Wczytuje wszystkie quizy z bazy i pokazuje w ListBoxie
        private void LoadBtn_Click(object sender, RoutedEventArgs e) => LoadQuizzes();

        private void LoadQuizzes()
        {
            // Pobieramy quizy razem z pytaniami i odpowiedziami
            var quizzes = _db.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .ToList();

            // Wyświetlamy w ListBoxie: Id, tytuł, liczba pytań
            QuizList.ItemsSource = quizzes.Select(q => $"{q.Id}: {q.Title} ({q.Questions.Count} pytań)").ToList();
        }

        // --- Zadanie 6: LINQ - filtrowanie quizów po tytule ---
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var term = TitleBox.Text.Trim();
            var query = _db.Quizzes.AsQueryable();

            if (!string.IsNullOrEmpty(term))
                query = query.Where(q => EF.Functions.Like(q.Title, $"%{term}%"));

            var result = query.ToList();
            QuizList.ItemsSource = result.Select(q => $"{q.Id}: {q.Title}").ToList();
        }

        // --- Zadanie 5: EF Core CRUD - READ (szczegóły quizu) ---
        // Podwójne kliknięcie na quiz w ListBoxie pokazuje pytania i odpowiedzi
        private void QuizList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (QuizList.SelectedItem == null) return;

            var selectedText = QuizList.SelectedItem.ToString();
            var id = int.Parse(selectedText.Split(':')[0]);

            // Pobieramy quiz z bazy (razem z pytaniami i odpowiedziami)
            _currentQuiz = _db.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .FirstOrDefault(q => q.Id == id);

            if (_currentQuiz != null)
            {
                QuizTitle.Text = _currentQuiz.Title;          // pokazujemy tytuł quizu
                QuestionsList.ItemsSource = _currentQuiz.Questions; // pokazujemy pytania i odpowiedzi
            }
        }

        // --- Zadanie 4: WPF + logika quizu ---
        // Sprawdza poprawność zaznaczonych odpowiedzi
        private void CheckBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuiz == null) return;

            int correct = 0;                          // licznik poprawnych odpowiedzi
            int total = _currentQuiz.Questions.Count; // liczba wszystkich pytań w quizie

            foreach (var question in _currentQuiz.Questions)
            {
                // Szukamy kontenera wizualnego dla danego pytania
                var container = QuestionsList.ItemContainerGenerator.ContainerFromItem(question) as FrameworkElement;
                if (container != null)
                {
                    // Pobieramy wszystkie RadioButtony (odpowiedzi) dla pytania
                    var radios = container.FindVisualChildren<System.Windows.Controls.RadioButton>();

                    // Szukamy zaznaczonego RadioButtona
                    var selected = radios.FirstOrDefault(r => r.IsChecked == true);

                    // Jeśli zaznaczona odpowiedź ma Tag = true (czyli IsCorrect), zwiększamy licznik poprawnych
                    if (selected != null && (bool)selected.Tag == true)
                        correct++;
                }
            }

            // Wynik pokazujemy w okienku MessageBox
            MessageBox.Show($"Twój wynik: {correct}/{total}");
        }
    }

    // --- Pomocnicza metoda do wyszukiwania kontrolek w drzewie wizualnym WPF ---
    // Dzięki niej możemy znaleźć wszystkie RadioButtony powiązane z danym pytaniem
    public static class VisualTreeHelperExtensions
    {
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);

                    // Jeśli dziecko jest typu T (np. RadioButton), zwracamy je
                    if (child is T t)
                        yield return t;

                    // Rekurencyjnie szukamy dalej w drzewie wizualnym
                    foreach (var childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
    }
}