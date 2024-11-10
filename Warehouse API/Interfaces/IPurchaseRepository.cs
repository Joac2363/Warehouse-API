using Warehouse_API.Models;

namespace Warehouse_API.Interfaces
{
    public interface IPurchaseRepository
    {
        Purchase GetPurchase(int purchaseId);
        ICollection<Purchase> GetAllPurchases();
        bool PurchaseExists(int purchaseId);
        bool CreatePurchase(Purchase purchase);
        bool DeletePurchase(Purchase purchase);
        int GetValueOfStock(int purchaseId);
    }
}
