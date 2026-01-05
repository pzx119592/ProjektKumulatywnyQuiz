using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        // W pamięci nic nie robimy – obiekt już jest zmieniony
    }

    public IReadOnlyList<T> GetAll()
    {
        return _items;
    }

    // 🔹 ASYNC – wymagane przez interfejs
    public Task<IReadOnlyList<T>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyList<T>>(_items);
    }

    public Task<T?> GetByIdAsync(int id)
    {
        var item = _items.FirstOrDefault(item =>
        {
            var prop = item?.GetType().GetProperty("Id");
            return prop != null && (int)prop.GetValue(item)! == id;
        });

        return Task.FromResult(item);
    }
}