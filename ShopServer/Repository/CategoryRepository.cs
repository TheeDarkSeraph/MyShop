using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Responses;
using ShopServer.Data;

namespace ShopServer.Repository {
    public class CategoryRepository(ShopDBContext context) : ICategory {

        public async Task<List<Category>> GetCategories() => await context.Categories.ToListAsync();
        
        public Task<ServiceResponse> AddCategory(Category model) {
            if (model == null)
                return Task.FromResult(ServiceResponse.Null);
            if (CategoryNameExists(model.Name))
                return Task.FromResult(ServiceResponse.MainNameExists);
            context.Categories.Add(model);
            context.SaveChanges();
            return Task.FromResult(ServiceResponse.Saved);
        }

        private bool CategoryNameExists(string catName) => context.Categories.Any(x => x.Name.ToLower() == catName.ToLower());

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
