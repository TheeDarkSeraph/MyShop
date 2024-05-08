using SharedModels;
using SharedModels.Contracts;
using SharedModels.Models;
using SharedModels.Responses;

namespace ShopClient.Services {
    public class ProductService (HttpClient httpClient) : IProduct {
        private const string productApi = "api/product";

        public async Task<List<Product>> GetProducts(bool featuredOnly) {
            var response = await httpClient.GetAsync($"{productApi}?featured={featuredOnly}");
            if (!response.IsSuccessStatusCode)
                return null!;
            var apiResponse = await response.Content.ReadAsStringAsync();

            Console.WriteLine("RESPONSE HERE " + apiResponse + " END response");
            return JsonUtils.DeserializeJsonString<List<Product>>(apiResponse);
        }

        public async Task<ServiceResponse> AddProduct(Product model) {
            var response = await httpClient.PostAsync(productApi,
                JsonUtils.GenerateStringContent(JsonUtils.SerializeObject(model)));
            if (!response.IsSuccessStatusCode)
                return ServiceResponse.Error;
            var apiResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Add RESPONSE HERE " + apiResponse + " END response");
            return JsonUtils.DeserializeJsonString<ServiceResponse>(apiResponse);
        }

        public async Task<ServiceResponse> DeleteProduct(int id) {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse> GetProduct(int id) {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse> UpdateProduct(Product model) {
            throw new NotImplementedException();
        }
    }
}
