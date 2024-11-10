using Microsoft.EntityFrameworkCore;
using Warehouse_API.Data;
using Warehouse_API.Interfaces;
using Warehouse_API.Models;

namespace Warehouse_API.Repositories
{
    public class PurchaseRepository : IPurchaseRepository, ISaveable
    {
        private readonly DataContext _context;

        public PurchaseRepository(DataContext context)
        {
            _context = context;
        }
        
        public ICollection<Purchase> GetAllPurchases()
        {
            return _context.Purchases.ToList();
        }

        public Purchase GetPurchase(int purchaseId)
        {
            return _context.Purchases.FirstOrDefault(p => p.PurchaseId == purchaseId);
        }
       
        public int GetValueOfStock(int purchaseId)
        {
            return _context.Orders
                .Include(o => o.Product)
                .Where(o => o.Purchase.PurchaseId == purchaseId)
                .Sum(o => o.Product.Price * o.Amount);
        }
       
        public bool PurchaseExists(int purchaseId)
        {
            return _context.Purchases.Any(p => p.PurchaseId == purchaseId);
        }
        
        public bool CreatePurchase(Purchase purchase)
        {
            _context.Add(purchase);
            return Save();
        }

        public bool DeletePurchase(Purchase purchase)
        {
            _context.Remove(purchase);
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
