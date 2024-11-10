using Warehouse_API.Models;
using Warehouse_API.DTO;

namespace Warehouse_API.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int PurchaseId { get; set; }
        public int Amount { get; set; }
    }
}
