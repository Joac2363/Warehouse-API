using Warehouse_API.Models;

namespace Warehouse_API.Interfaces
{
    public interface IStockRepository
    {
        bool CreateStock(Stock stock);
        bool UpdateStock(Stock stock);
        bool DeleteStock(Stock stock);
    }
}
