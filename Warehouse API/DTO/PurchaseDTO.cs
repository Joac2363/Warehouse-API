namespace Warehouse_API.DTO
{
    public class PurchaseDTO
    {
        public int PurchaseId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? FulfilmentDate { get; set; }
    }
}
