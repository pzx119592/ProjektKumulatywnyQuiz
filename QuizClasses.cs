using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// --- Zadanie 2: Interfejsy i abstrakcja ---
// Interfejs IAnswer definiuje kontrakt dla odpowiedzi w quizie.
// Dzięki temu możemy traktować różne implementacje odpowiedzi w ten sam sposób.
public interface IAnswer
{
    int Id { get; set; }              // Klucz główny (dla EF Core)
    string Text { get; set; }         // Treść odpowiedzi
    bool IsCorrect { get; set; }      // Flaga: czy odpowiedź jest poprawna
}

// Interfejs IQuestion definiuje kontrakt dla pytania.
// Dzięki temu możemy pisać kod, który działa na "pytaniach" niezależnie od implementacji.
public interface IQuestion
{
    int Id { get; set; }              // Klucz główny
    string Text { get; set; }         // Treść pytania
    List<Answer> Answers { get; set; } // Lista odpowiedzi powiązanych z pytaniem
}

// Interfejs IQuiz definiuje kontrakt dla quizu.
// Abstrakcja pozwala nam oddzielić logikę od implementacji.
public interface IQuiz
{
    int Id { get; set; }              // Klucz główny
    string Title { get; set; }        // Tytuł quizu
    List<Question> Questions { get; set; } // Lista pytań w quizie
}

// --- Zadanie 3: Generyki ---
// Interfejs generyczny IRepository<T> definiuje podstawowe operacje CRUD.
// Dzięki generykom możemy używać tego samego repozytorium dla różnych typów (Quiz, Question, Answer).
public interface IRepository<T> where T : class
{
    T? GetById(int id);               // Pobranie obiektu po Id
    List<T> GetAll();                 // Pobranie wszystkich obiektów
    void Add(T entity);               // Dodanie nowego obiektu
    void Update(T entity);            // Aktualizacja obiektu
    void Delete(T entity);            // Usunięcie obiektu
}

// --- Zadanie 1: Programowanie obiektowe ---
// Klasa Answer implementuje interfejs IAnswer.
// Reprezentuje pojedynczą odpowiedź w quizie.
public class Answer : IAnswer
{
    public int Id { get; set; }                        // Klucz główny dla EF Core
    public string Text { get; set; } = string.Empty;   // Treść odpowiedzi
    public bool IsCorrect { get; set; }                // Czy odpowiedź jest poprawna
}

// Klasa Question implementuje interfejs IQuestion.
// Reprezentuje pojedyncze pytanie w quizie.
public class Question : IQuestion
{
    public int Id { get; set; }                        // Klucz główny dla EF Core
    public string Text { get; set; } = string.Empty;   // Treść pytania
    public List<Answer> Answers { get; set; } = new(); // Lista odpowiedzi
}

// Klasa Quiz implementuje interfejs IQuiz.
// Reprezentuje cały quiz (tytuł + lista pytań).
public class Quiz : IQuiz
{
    public int Id { get; set; }                        // Klucz główny dla EF Core
    public string Title { get; set; } = string.Empty;  // Tytuł quizu
    public List<Question> Questions { get; set; } = new(); // Lista pytań
}