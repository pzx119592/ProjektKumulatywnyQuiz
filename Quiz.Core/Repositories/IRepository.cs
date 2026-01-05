using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quiz.Core.Repositories;

public interface IRepository<T>
{
    void Add(T item);
    void Remove(T item);
    void Update(T item);

    IReadOnlyList<T> GetAll();
    Task<IReadOnlyList<T>> GetAllAsync();

    Task<T?> GetByIdAsync(int id);
}