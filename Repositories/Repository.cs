using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

// Używamy tej samej przestrzeni nazw co reszta projektu, żeby było prościej
namespace ProjektKumulatywnyQuiz
{
    // Ta klasa "wypełnia" obietnicę złożoną w IRepository
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly QuizDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(QuizDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }
    }
}