using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Responses;
using ShopServer.Data;

namespace ShopServer.Repository {
    public class CategoryRepository(ShopDBContext context) : ICategory {

        public async Task<List<Category>> GetCategories() => await context.Categories.ToListAsync();
        
        public async Task<ServiceResponse> AddCategory(Category model) {
            if (model == null)
                return ServiceResponse.Null;
            if (CategoryNameExists(model.Name))
                return ServiceResponse.MainNameExists;
            context.Categories.Add(model);
            await context.SaveChangesAsync();
            return ServiceResponse.Saved;
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
