namespace Warehouse_API.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int SKU { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public ICollection<Stock> Stock { get; set; }
    }
}
