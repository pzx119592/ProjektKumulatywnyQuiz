using System;
using System.Collections.Generic;
using System.Linq;
using Quiz.Core.Repositories;

namespace Quiz.ConsoleApp.Repositories;

public class InMemoryRepository<T> : IRepository<T>
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }

    public void Update(T item)
    {
        // InMemory:
        // obiekt jest już zmodyfikowany w pamięci,
        // więc nie trzeba nic robić
    }

    public IReadOnlyList<T> GetAll()
    {
        return _items;
    }

    public T? GetById(int id)
    {
        return _items.FirstOrDefault(item =>
        {
            var property = item?.GetType().GetProperty("Id");
            if (property == null) return false;
            return (int)property.GetValue(item)! == id;
        });
    }
}