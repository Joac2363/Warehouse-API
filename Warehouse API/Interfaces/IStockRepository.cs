using Warehouse_API.Models;

namespace Warehouse_API.Interfaces
{
    public interface IStockRepository
    {
        Stock GetStock(int productId, int warehouseId);
        ICollection<Stock> GetAllStock();
        ICollection<Stock> GetAllStockAtWarehouse(int warehouseId);
        bool CreateStock(Stock stock);
        bool UpdateStock(Stock stock);
        bool DeleteStock(Stock stock);
        bool MoveStock(Stock stock, Warehouse toWarehouse, int amount);
        bool StockExists(int productId, int warehouseId);
        bool StockExists(Stock stock);
    }
}
