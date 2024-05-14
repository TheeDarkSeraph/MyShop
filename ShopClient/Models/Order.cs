namespace ShopClient.Models {
    public class Order {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string? Image { get; set; }
        public decimal SubTotal => Price * Amount;
    }
}
