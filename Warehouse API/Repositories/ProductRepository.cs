using Microsoft.EntityFrameworkCore.Diagnostics;
using Warehouse_API.Data;
using Warehouse_API.Helpers;
using Warehouse_API.Interfaces;
using Warehouse_API.Models;

namespace Warehouse_API.Repositories
{
    public class ProductRepository : IProductRepository, ISaveable
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateProduct(Product product)
        {
            if (product.SKU == 0)
            {
                SKUHelper skuhelp = new SKUHelper(product.Price);
                int sku = skuhelp.New();
                while (_context.Products.Where(p => p.SKU == sku).Any())
                {
                    sku = skuhelp.New();
                } 
                product.SKU = sku;
            }
            _context.Add(product);

            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Remove(product);
            return Save();
        }
        public bool DeleteProduct(int productId)
        {
            return DeleteProduct(_context.Products.Where(p => p.ProductId == productId).FirstOrDefault());
        }

        public ICollection<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProduct(int productId)
        {
            return _context.Products.Where(p => p.ProductId == productId).FirstOrDefault();
        }

        public int GetProductPrice(int productId)
        {
            return GetProduct(productId).Price;
        }

        public bool ProductExists(int productId)
        {
            return _context.Products.Any(p => p.ProductId == productId);
        }


        public bool UpdateProduct(Product product)
        {
            _context.Update(product);
            return Save();
        }
        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
