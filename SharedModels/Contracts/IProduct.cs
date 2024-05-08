using SharedModels.Models;
using SharedModels.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Contracts {
    public interface IProduct {
        Task<List<Product>> GetProducts(bool featuredOnly);
        Task<ServiceResponse> AddProduct(Product model);
        Task<ServiceResponse> GetProduct(int id);
        Task<ServiceResponse> UpdateProduct(Product model);
        Task<ServiceResponse> DeleteProduct(int id);
    }
}
