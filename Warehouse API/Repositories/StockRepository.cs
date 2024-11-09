using Warehouse_API.Data;
using Warehouse_API.Interfaces;
using Warehouse_API.Models;

namespace Warehouse_API.Repositories
{
    public class StockRepository : IStockRepository, ISaveable
    {
        private readonly DataContext _context;

        public StockRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateStock(Stock stock)
        {
            _context.Add(stock);
            return Save();
        }

        public bool DeleteStock(Stock stock)
        {
            _context.Remove(stock);
            return Save();
        }

        public bool UpdateStock(Stock stock)
        {
            _context.Update(stock);
            return Save();
        }
        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
