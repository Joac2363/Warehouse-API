namespace Warehouse_API.Models
{
    public class Stock
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int MinAcceptableStock { get; set; }
        public int Amount { get; set; }
    }
}
