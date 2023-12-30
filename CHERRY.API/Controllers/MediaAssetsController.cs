using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.MediaAssets;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaAssetsController : ControllerBase
    {
        private readonly IMediaAssetsService _IMediaAssetsService;
        private readonly Cloudinary _cloudinary;

        public MediaAssetsController(IMediaAssetsService IMediaAssetsService, Cloudinary cloudinary)
        {
            _IMediaAssetsService = IMediaAssetsService;
            _cloudinary = cloudinary;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateImage([FromForm] MediaAssetsCreateVM request)
        {
            try
            {
                var imageIds = await _IMediaAssetsService.CreateAsync(request, _cloudinary);
                return Ok(imageIds);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }
    }
}
