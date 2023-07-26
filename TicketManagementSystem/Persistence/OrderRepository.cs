using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Persistence
{
    public class OrderRepository : IOrderRepository
    {

        private readonly TicketManagementSystemContext _dbcontext;
        public OrderRepository()
        {
            _dbcontext = new TicketManagementSystemContext();
        }
        public void Add(Order entity)
        {
            throw new NotImplementedException();
        }

        public Order Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetAll()
        {
            return _dbcontext.Orders
                .Include(o => o.TicketCategory)
                .ToList();
        }

        public Order GetById(int id)
        {
            return _dbcontext.Orders
                    .Include(o => o.TicketCategory)
                    .Where(o => o.Orderid == id)
                    .First();
        }

        public Order Update(Order entity)
        {
            var entry = _dbcontext.Update(entity);
            _dbcontext.SaveChanges();
            return entry.Entity;
        }
    }
}
