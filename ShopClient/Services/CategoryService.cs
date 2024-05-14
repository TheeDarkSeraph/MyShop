using SharedModels;
using SharedModels.Models;
using SharedModels.Responses;
using ShopClient.Auth;

namespace ShopClient.Services {
    public class CategoryService(HttpClient httpClient) : ICategoryService { // not shared among different users
        private const string categoryApi = "api/category";
        /* In a Blazor application, each user session is typically isolated, 
         *      and the components and services used within the session maintain their own state.
         * If the data variable is stored within a component or service used within the user's session, 
         *      it will be accessible and modifiable only within that session. Other users accessing the same Blazor 
         *      page will have their own separate instances of the component or service, which means they will have 
         *      their own separate data variables.
         */
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
