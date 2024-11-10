using Warehouse_API.Models;

namespace Warehouse_API.DTO
{
    public class StockDTO
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int MinAcceptableStock { get; set; }
        public int Amount { get; set; }
    }
}
