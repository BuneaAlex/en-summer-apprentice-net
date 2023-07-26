using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Persistence
{
    public class OrderRepository : IOrderRepository
    {

        private readonly TicketManagementSystemContext _dbcontext;
        public OrderRepository(TicketManagementSystemContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Add(Order entity)
        {
            throw new NotImplementedException();
        }

        public Order Delete(int id)
        {
            var order = GetById(id);
            if(order != null)
            {
                _dbcontext.Remove(order);
                _dbcontext.SaveChanges();
            }
            return order;
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
                        .ThenInclude(tc => tc.Event)
                            .ThenInclude(ev => ev.Venue)
                    .Include(o => o.TicketCategory)
                        .ThenInclude(tc => tc.Event)
                        .ThenInclude(ev => ev.EventType)
                    .Include(o => o.Customer)
                    .Where(o => o.Orderid == id)
                    .FirstOrDefault();
        }

        public Order Update(Order entity)
        {
            _dbcontext.Update(entity);
            _dbcontext.SaveChanges();
            return entity;
        }
    }
}
