using System.Linq.Expressions;

namespace TicketManagementSystem.Persistence
{
    public interface IRepository<T,ID> where T : class
    {
        T GetById(ID id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        T Update(T entity);
        T Delete(ID id);
    }
}
