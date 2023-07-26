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

        public async Task<Order> Delete(int id)
        {
            var order = await GetById(id);
            if (order != null)
            {
                _dbcontext.Remove(order);
                await _dbcontext.SaveChangesAsync();
            }
            return order;
        }

        public IEnumerable<Order> GetAll()
        {
            return _dbcontext.Orders
                .Include(o => o.TicketCategory)
                .ToList();
        }

        public async Task<Order> GetById(int id)
        {
            return await _dbcontext.Orders
                .Include(o => o.TicketCategory)
                    .ThenInclude(tc => tc.Event)
                        .ThenInclude(ev => ev.Venue)
                .Include(o => o.TicketCategory)
                    .ThenInclude(tc => tc.Event)
                        .ThenInclude(ev => ev.EventType)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Orderid == id);
        }

        public async Task<Order> Update(Order entity)
        {
            _dbcontext.Update(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }
    }
}
