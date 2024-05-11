using SharedModels.Models;
using SharedModels.Responses;

namespace ShopServer.Repository {
    public interface ICategory {
        Task<List<Category>> GetCategories();
        Task<ServiceResponse> AddCategory(Category model);
        Task<ServiceResponse> GetCategory(int id);
        Task<ServiceResponse> UpdateCategory(Category model);
        Task<ServiceResponse> DeleteCategory(int id);
    }
}
