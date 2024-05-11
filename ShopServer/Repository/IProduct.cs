using SharedModels.Models;
using SharedModels.Responses;

namespace SharedModels.Repository {
    // holds product information
    public interface IProduct {
        Task<List<Product>> GetProducts(bool featuredOnly);
        Task<ServiceResponse> AddProduct(Product model);
        Task<ServiceResponse> GetProduct(int id);
        Task<ServiceResponse> UpdateProduct(Product model);
        Task<ServiceResponse> DeleteProduct(int id);
    }
}
