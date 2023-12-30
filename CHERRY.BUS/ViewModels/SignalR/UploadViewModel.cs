using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CHERRY.BUS.ViewModels.SignalR
{
    public class UploadViewModel
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
