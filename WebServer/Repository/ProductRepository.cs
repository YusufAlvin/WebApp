using WebServer.Model;

namespace WebServer.Repository;

public class ProductRepository: IProductRepository
{
    private readonly AppDbContext _dbContext;
    
    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        var products = _dbContext.Products.ToList();
        return products;
    }

    public void AddProduct(Product product)
    {
        if (_dbContext.Products != null && _dbContext.Products.Any(p => p.Name == product.Name))
        {
            throw new ArgumentException("Product name must be unique");
        }

        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();
    }

    public Product FindProductById(int id)
    {
        var product = _dbContext.Products.Where(p => p.Id == id).FirstOrDefault();
        return product;
    }
}