using Blazored.LocalStorage;
using SharedModels;
using SharedModels.Models;
using SharedModels.Responses;
using ShopClient.Models;
using Syncfusion.Blazor;

namespace ShopClient.Services {
    public class CartService(HttpClient httpClient, ILocalStorageService localStorage, IProductService productService) : ICart {
        public Action? CartAction { get; set; }
        public int CartCount { get; set; }
        public bool IsCartLoaderVisible { get; set; }


        public async Task GetCartCount() {
            string cartString = await GetCartFromLocalStorage();
            if (string.IsNullOrEmpty(cartString))
                CartCount = 0;
            else
                CartCount = JsonUtils.DeserializeJsonString<List<StorageCart>>(cartString).Count();
            CartAction?.Invoke();
        }

        public async Task<ServiceResponse> AddToCart(Product model, int updateQuantity = 1) {
            string msg = string.Empty;
            List<StorageCart> myCart;
            var storageCartString = await GetCartFromLocalStorage();
            if (!string.IsNullOrEmpty(storageCartString)) {
                myCart = JsonUtils.DeserializeJsonString<List<StorageCart>>(storageCartString);
                var checkIfAddedAlready = myCart.FirstOrDefault(x => x.ProductId == model.Id);
                if (checkIfAddedAlready == null) {
                    myCart.Add(new StorageCart { ProductId = model.Id, Amount = updateQuantity });
                    msg = "Product Added to Cart";
                } else {
                    checkIfAddedAlready.Amount = updateQuantity;
                    msg = "Product Quantity Updated";
                }
            } else {
                myCart = [new StorageCart { ProductId = model.Id, Amount = updateQuantity }];
                msg = "Product Added to Cart";
            }
            await SetCartToLocalStorage(JsonUtils.SerializeObject(myCart)); // override
            await GetCartCount();
            return new ServiceResponse(true, msg);
        }

        public async Task<List<Order>> MyOrders() {
            IsCartLoaderVisible = true;
            List<Order> orderList = new();
            string myCartString = await GetCartFromLocalStorage();
            if(string.IsNullOrEmpty(myCartString)) {
                IsCartLoaderVisible = false;
                return null!;
            }
            List<StorageCart> myCartList = JsonUtils.DeserializeJsonString<List<StorageCart>>(myCartString);
            List<Product> AllProducts =  await productService.GetProducts(false);
            foreach (var cartItem in myCartList){
                Product? product = AllProducts.FirstOrDefault(x => x.Id == cartItem.ProductId);
                if(product==null) // just in case there is a problem, or some kind of an attack
                    continue;
                if (cartItem != null) {
                    orderList.Add(new Order {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Amount = cartItem.Amount,
                        Image = product.Base64Img
                    });
                }
            }
            IsCartLoaderVisible = false;
            await GetCartCount();
            return orderList;
        }

        public async Task<ServiceResponse> DeleteCart(Order cart) {
            List<StorageCart> myCartList = JsonUtils.DeserializeJsonString<List<StorageCart>>(await GetCartFromLocalStorage());
            if(myCartList == null)
                return ServiceResponse.Null;
            myCartList.Remove(myCartList.FirstOrDefault(x => x.ProductId == cart.Id)!);
            await SetCartToLocalStorage(JsonUtils.SerializeObject(myCartList));
            await GetCartCount();
            return ServiceResponse.ProductRemoved;
        }
        private async Task<string> GetCartFromLocalStorage() => await localStorage.GetItemAsStringAsync("cart");
        private async Task SetCartToLocalStorage(string cart) => await localStorage.SetItemAsStringAsync("cart", cart);
        private async Task RemoveCartFromLocalStorage() => await localStorage.RemoveItemAsync("cart");
    }
}
