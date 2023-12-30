using Microsoft.AspNetCore.Http;

namespace CHERRY.BUS.ViewModels.MediaAssets
{
    public class MediaAssetsCreateVM
    {
        public string CreateBy { get; set; }
		public Guid? IDVariant { get; set; }
		public Guid? IDReview { get; set; }
		public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? MimeType { get; set; }
        public string? Path { get; set; } 
        public List<IFormFile> ImageFile { get; set; } = null!;
        public string? AccessLevel { get; set; }
        public string? Tags { get; set; }
        public string? AltText { get; set; }
        public int Status { get; set; }
    }
}
