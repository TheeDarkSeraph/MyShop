using SharedModels.Models;
using SharedModels.Responses;

namespace ShopClient.Services {
    public interface ICategoryService {
        //Action? CategoryAction { get; set; }
        List<Category> AllCategories { get; set; }
        Task RefreshCategories();
        Task GetAndCacheCategories();
        Task<List<Category>> GetCategories();
        Task<ServiceResponse> AddCategory(Category model);
        Task<ServiceResponse> GetCategory(int id);
        Task<ServiceResponse> UpdateCategory(Category model);
        Task<ServiceResponse> DeleteCategory(int id);
    }
}
