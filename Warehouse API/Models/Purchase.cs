namespace Warehouse_API.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? FulfilmentDate { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
