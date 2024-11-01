using MultiTenantApp.Models;
using MultiTenantApp.Services.ProductService.DTOs;

namespace MultiTenantApp.Services.ProductService
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        Product CreateProduct(CreateProductRequest request);
        bool DeleteProduct(int id);
    }
}
