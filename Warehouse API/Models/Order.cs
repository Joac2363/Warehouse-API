namespace Warehouse_API.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public int ProductId { get; set; }
        public int PurchaseId { get; set; }
        public Product Product { get; set; }
        public Purchase Purchase { get; set; }
    }
}
