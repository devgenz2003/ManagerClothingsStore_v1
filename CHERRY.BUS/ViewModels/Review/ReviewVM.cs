
using CHERRY.BUS.ViewModels.Variants;
using Microsoft.AspNetCore.Http;

namespace CHERRY.BUS.ViewModels.Review
{
    public class ReviewVM
    {
        public Guid ID { get; set; }
        public string IDUser { get; set; } = null!;
        public string Username { get; set; } = null!;
        public Guid IDVariant { get; set; }
        public Guid? IDOrderVariant { get; set; }
        public string Content { get; set; } = null!;
        public int Rating { get; set; }
        public bool IsPurchased { get; set; }
        public int Status { get; set; }
        public List<string> ImagePaths { get; set; }
        public DateTime CreateDate { get; set; }
        public string SizeName { get; set; } 
        public string ColorName { get; set; } 
    }
}
