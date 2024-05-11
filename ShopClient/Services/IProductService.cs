using SharedModels.Models;
using SharedModels.Responses;

namespace ShopClient.Services {
    public interface IProductService {
        Task<List<Product>> GetProducts(bool featuredOnly);
        Task<ServiceResponse> AddProduct(Product model);
        Task<ServiceResponse> GetProduct(int id);
        Task<ServiceResponse> UpdateProduct(Product model);
        Task<ServiceResponse> DeleteProduct(int id);
        Task<List<Product>> GetProductsByCategory(int categoryId);
    }
}
