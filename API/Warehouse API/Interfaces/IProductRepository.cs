using Warehouse_API.Models;

namespace Warehouse_API.Interfaces
{
    public interface IProductRepository
    {
        bool ProductExists(int productId);
        ICollection<Product> GetAllProducts();
        Product GetProduct(int productId);
        int GetProductPrice(int productId);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        bool DeleteProduct(int productId);

    }
}
