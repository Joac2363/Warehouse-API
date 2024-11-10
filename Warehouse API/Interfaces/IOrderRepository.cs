using Warehouse_API.Models;

namespace Warehouse_API.Interfaces
{
    public interface IOrderRepository
    {
        Order GetOrder(int orderId);
        ICollection<Order> GetAllOrders();
        bool CreateOrder(Order order);
        bool DeleteOrder(Order order);
        bool OrderExists(int orderId);
    }
}
