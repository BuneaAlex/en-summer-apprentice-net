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

        public IEnumerable<Order> GetAll()
        {
            return _dbcontext.Orders
                .Include(o => o.TicketCategory)
                .ToList();
        }

        public Order GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Order entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
