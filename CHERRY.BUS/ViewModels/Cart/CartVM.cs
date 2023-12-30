using CHERRY.BUS.ViewModels.CartProductVariants;

namespace CHERRY.BUS.ViewModels.Cart
{
    public class CartVM
    {
        public Guid ID { get; set; }
        public string ID_User { get; set; }
        public int Status { get; set; }
        public List<CartProductVariantsVM> Items { get; set; }

    }
}
