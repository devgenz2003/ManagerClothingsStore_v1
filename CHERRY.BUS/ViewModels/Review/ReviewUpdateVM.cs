
namespace CHERRY.BUS.ViewModels.Review
{
    public class ReviewUpdateVM
    {
        public string? ModifiedBy { get; set; }
        public Guid ID { get; set; }
        public string IDUser { get; set; } = null!;
        public Guid IDVariant { get; set; }
        public Guid? IDOrderVariant { get; set; }
        public string Content { get; set; } = null!;
        public int Rating { get; set; }
        public bool IsPurchased { get; set; }
        public int Status { get; set; }
    }
}
