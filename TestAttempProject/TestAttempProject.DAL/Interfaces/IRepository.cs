using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAttemptProject.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();
        public Task<T> GetAsync(int id);
        public Task AddAsync(T item);
        public Task UpdateAsync(T item);
        public Task DeleteAsync(int itemId);
    }
}
