using Microsoft.AspNetCore.Http;

namespace CHERRY.BUS.ViewModels.MediaAssets
{
    public class MediaAssetsVM
    {
        public Guid ID { get; set; }
        public Guid? IDProduct { get; set; }
        public Guid? IDReview { get; set; }
        public Guid? IDProductVariants { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? MimeType { get; set; }
        public byte[] Path { get; set; }
        public string? AccessLevel { get; set; }
        public string? Tags { get; set; }
        public string? AltText { get; set; }
        public int Status { get; set; }
    }
}
