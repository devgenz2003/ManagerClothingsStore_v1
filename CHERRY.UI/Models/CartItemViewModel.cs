namespace CHERRY.UI.Models
{
    public class CartItemViewModel
    {
        public Guid IDOptions { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price;
    }
}
