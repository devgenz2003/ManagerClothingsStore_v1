namespace CHERRY.BUS.ViewModels.CartProductVariants
{
    public class CartProductVariantsUpdateVM
    {
        public string? MODIFIEDBY { get; set; }
        public Guid? IDVariant { get; set; }
        public Guid ID_Cart { get; set; }
        public int Quantity { get; set; }
        public decimal Total_Amount { get; set; }
        public string? Notes { get; set; }
        public int Status { get; set; }
        public bool IsVariant { get; set; }
        public bool IsProduct { get; set; }

    }
}
