using SharedModels;
using SharedModels.Models;
using SharedModels.Responses;

namespace ShopClient.Services {
    public class CategoryService(HttpClient httpClient) : ICategoryService {
        private const string categoryApi = "api/category";

        //public Action? CategoryAction { get; set; }
        public List<Category> AllCategories { get; set; } = new List<Category>();

        //Cache categories (since they will be less)

        public async Task GetAndCacheCategories() {
            if (AllCategories.Count == 0)
                await RefreshCategories();
        }
        public async Task RefreshCategories() => AllCategories = await GetCategories(); // can force call
        public async Task<List<Category>> GetCategories() {
            var response = await httpClient.GetAsync(categoryApi);
            if (!response.IsSuccessStatusCode)
                return null!;
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonUtils.DeserializeJsonString<List<Category>>(apiResponse);
        }

        



        public async Task<ServiceResponse> AddCategory(Category model) {
            var response = await httpClient.PostAsync(categoryApi,
                JsonUtils.GenerateStringContent(JsonUtils.SerializeObject(model)));
            if (!response.IsSuccessStatusCode)
                return ServiceResponse.Error;
            var apiResponse = await response.Content.ReadAsStringAsync();
            await RefreshCategories();
            return JsonUtils.DeserializeJsonString<ServiceResponse>(apiResponse);
        }

        public Task<ServiceResponse> DeleteCategory(int id) {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> GetCategory(int id) {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> UpdateCategory(Category model) {
            throw new NotImplementedException();
        }
    }
}
