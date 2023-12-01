using Microsoft.AspNetCore.Mvc;
using WebServer.Model;
using WebServer.Repository;

namespace WebServer.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController
{
    private readonly IProductRepository _productRepository;
    
    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    [HttpGet]
    public IEnumerable<Product> GetAllProducts()
    {
        var products = _productRepository.GetAllProducts();
        return products;
    }
    
    [HttpPost("add-product")]
    public void AddProduct(Product product)
    {
        _productRepository.AddProduct(product);
    }
}