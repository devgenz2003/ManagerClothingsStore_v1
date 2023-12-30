
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CHERRY.BUS.ViewModels.Review
{
    public class ReviewCreateVM
    {
        public string CreateBy { get; set; }
        public Guid ID { get; set; } = Guid.NewGuid();
        public string IDUser { get; set; } = null!;
        public Guid IDVariant { get; set; }
        public Guid? IDOrderVariant { get; set; }
        public string Content { get; set; } = null!;
        public int Rating { get; set; }
        public bool IsPurchased { get; set; }
        public int Status { get; set; }
        public List<IFormFile> ImagePaths { get; set; }
    }

}
