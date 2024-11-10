using AutoMapper;
using Warehouse_API.Data;
using Warehouse_API.Interfaces;
using Warehouse_API.Models;

namespace Warehouse_API.Repositories
{
    public class StockRepository : IStockRepository, ISaveable
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public StockRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Stock GetStock(int productId, int warehouseId)
        {
            return _context.Stock.Where(p => p.ProductId == productId).Where(w => w.WarehouseId == warehouseId).FirstOrDefault();
        }
        public ICollection<Stock> GetAllStock()
        {
            return _context.Stock.ToList();
        }
        public ICollection<Stock> GetAllStockAtWarehouse(int warehouseId)
        {
            return _context.Stock.Where(w => w.WarehouseId == warehouseId).ToList();
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
        
        public bool MoveStock(Stock stock, Warehouse toWarehouse, int amount)
        {
            stock.Amount -= amount;

            // Create a new stock, or edit an existing one
            Stock recieverStock;
            if (!StockExists(stock.ProductId, toWarehouse.WarehouseId))
            {
                recieverStock = new Stock();
                recieverStock.ProductId = stock.ProductId;
                recieverStock.Product = stock.Product;
                recieverStock.WarehouseId = toWarehouse.WarehouseId;
                recieverStock.Warehouse = toWarehouse;
                recieverStock.Amount = amount;
                recieverStock.MinAcceptableStock = 0;
                _context.Add(recieverStock);
            } else
            {
                recieverStock = GetStock(stock.ProductId, toWarehouse.WarehouseId);
                recieverStock.Amount += amount;
                _context.Update(recieverStock);
            }

            // Update or remove the giving stock
            if (stock.Amount == 0)
            {
                _context.Remove(stock);
            } else
            {
                _context.Update(stock);
            }
            return Save();
            
        }


        public bool StockExists(int productId, int warehouseId)
        {
            return _context.Stock.Where(p => p.ProductId == productId).Where(w => w.WarehouseId == warehouseId).Any();
        }

        public bool StockExists(Stock stock)
        {
            return StockExists(stock.ProductId, stock.WarehouseId);
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
