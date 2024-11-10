using Warehouse_API.Data;
using Warehouse_API.Interfaces;
using Warehouse_API.Models;

namespace Warehouse_API.Repositories
{
    public class OrderRepository : IOrderRepository, ISaveable
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }

        public Order GetOrder(int orderId)
        {
            return _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
        }
        
        public bool OrderExists(int orderId)
        {
            return _context.Orders.Any(o => o.OrderId == orderId);
        }
        
        public bool CreateOrder(Order order)
        {
            _context.Add(order);
            return Save();
        }

        public bool DeleteOrder(Order order)
        {
            _context.Remove(order);
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
