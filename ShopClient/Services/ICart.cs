using SharedModels.Models;
using SharedModels.Responses;
using ShopClient.Models;

namespace ShopClient.Services {
    public interface ICart {
        public Action? CartAction { get; set; }
        public int CartCount { get; set; }
        public bool IsCartLoaderVisible { get; set; }
        Task GetCartCount();
        Task<ServiceResponse> AddToCart(Product model, int updateQuantity = 1);
        Task<List<Order>> MyOrders();
        Task<ServiceResponse> DeleteCart(Order cart);
    }
}
