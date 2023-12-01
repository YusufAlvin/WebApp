using WebServer.Model;

namespace WebServer.Repository;

public interface IProductRepository
{
    IEnumerable<Product> GetAllProducts();
    void AddProduct(Product product);
    Product FindProductById(int id);
}