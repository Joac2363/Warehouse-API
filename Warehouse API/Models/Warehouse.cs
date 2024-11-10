namespace Warehouse_API.Models
{
    public class Warehouse
    {
        public int WarehouseId { get; set; }
        public string Name { get; set; }
        public int Capacity{ get; set; }
        public ICollection<Stock> Stock { get; set; }
    }
}

