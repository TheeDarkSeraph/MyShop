using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Responses;
using ShopServer.Data;

namespace SharedModels.Repository {
    public class ProductRepository (ShopDBContext context): IProduct {
        public async Task<List<Product>> GetProducts(bool featuredOnly) {
            if (featuredOnly)
                return await context.Products.Where(x => x.Featured).ToListAsync();
            return await context.Products.ToListAsync();
        }

        public async Task<ServiceResponse> AddProduct(Product model) {
            if (model == null)
                return ServiceResponse.Null;
            if (ProductNameExists(model.Name))
                return ServiceResponse.MainNameExists;
            context.Products.Add(model);
            await context.SaveChangesAsync();
            return ServiceResponse.Saved;
        }
        private bool ProductNameExists(string productName) => context.Products.Any(x => x.Name.ToLower() == productName.ToLower());

        public Task<ServiceResponse> DeleteProduct(int id) {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> GetProduct(int id) {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> UpdateProduct(Product model) {
            throw new NotImplementedException();
        }
    }
}
