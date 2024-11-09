using Warehouse_API.Data;
using Warehouse_API.Interfaces;
using Warehouse_API.Models;

namespace Warehouse_API.Repositories
{
    public class WarehouseRepository : IWarehouseRepository, ISaveable
    {
        private readonly DataContext _context;

        public WarehouseRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateWarehouse(Warehouse warehouse)
        { 
            _context.Add(warehouse);
            return Save();
        }
        public bool WarehouseExists(int warehouseId)
        {
            return _context.Warehouses.Any(w => w.WarehouseId == warehouseId);
        }

        public bool DeleteWarehouse(Warehouse warehouse)
        {
            _context.Remove(warehouse);
            return Save();
        }
        public bool DeleteWarehouse(int warehouseId)
        {
            return DeleteWarehouse(_context.Warehouses.Where(w => w.WarehouseId == warehouseId).FirstOrDefault());
        }

        public ICollection<Product> GetAllProducts(int warehouseId)
        {
            return _context.Stock.Where(w => w.WarehouseId==warehouseId).Select(p => p.Product).ToList();
        }

        public ICollection<Warehouse> GetAllWarehouses()
        {
            return _context.Warehouses.ToList();
        }

        public int GetTotalStock(int warehouseId)
        {
            return _context.Stock.Where(w => w.WarehouseId == warehouseId).Select(p => p.Amount).Sum();
        }

        public int GetTotalValueOfStock(int warehouseId)
        {
            int value = 0;
            foreach(Stock stock in _context.Stock.Where(w => w.WarehouseId == warehouseId))
            {
                value += stock.Product.Price * stock.Amount;
            }
            return value;
        }

        public Warehouse GetWarehouse(int warehouseId)
        {
            return _context.Warehouses.Where(w => w.WarehouseId == warehouseId).FirstOrDefault();
        }

        public int GetWarehouseCapacity(int warehouseId)
        {
            return _context.Warehouses.Where(w => w.WarehouseId == warehouseId).Select(w => w.Capacity).FirstOrDefault();
        }
        public bool UpdateWarehouse(Warehouse warehouse)
        {
            _context.Update(warehouse);
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
