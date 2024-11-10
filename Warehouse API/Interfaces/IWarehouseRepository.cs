using Warehouse_API.Models;

namespace Warehouse_API.Interfaces
{
    public interface IWarehouseRepository
    {
        public ICollection<Warehouse> GetAllWarehouses();
        public Warehouse GetWarehouse(int warehouseId);
        public bool WarehouseExists(int warehouseId);
        public int GetWarehouseCapacity(int warehouseId);
        public ICollection<Product> GetAllProducts(int warehouseId);
        public int GetTotalStock(int warehouseId);
        public int GetTotalValueOfStock(int warehouseId);

        public bool CreateWarehouse(Warehouse warehouse);
        public bool UpdateWarehouse(Warehouse warehouse);
        public bool DeleteWarehouse(Warehouse warehouse);
        public bool DeleteWarehouse(int warehouseId);

    }
}
