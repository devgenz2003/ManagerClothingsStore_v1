using Microsoft.AspNetCore.Mvc.Rendering;

namespace CHERRY.UI.Models
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid UserID { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public IEnumerable<SelectListItem> ShippingMethods { get; set; }
        public IEnumerable<SelectListItem> PaymentMethods { get; set; }
    }
}
