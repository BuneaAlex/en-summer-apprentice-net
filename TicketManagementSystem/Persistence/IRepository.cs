using System.Linq.Expressions;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Persistence
{
    public interface IRepository<T,ID> where T : class
    {
        Task<T> GetById(ID id);
        Task<IEnumerable<T>> GetAll();
        void Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(ID id);
    }
}
