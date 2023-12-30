using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.Services._2_Implements;
using CHERRY.BUS.ViewModels.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly IOptionsService _IOptionsService;
        private readonly IMemoryCache _cache;
        public OptionsController(IOptionsService IOptionsService, IMemoryCache cache)
        {
            _IOptionsService = IOptionsService ?? throw new ArgumentNullException(nameof(IVariantsService));
            _cache = cache;
        }
        [HttpGet("FindIDOptions")]
        public async Task<ActionResult<Guid?>> FindIDOptionsAsync(Guid IDVariant, string size, string color)
        {
            try
            {
                var option = await _IOptionsService.FindIDOptionsAsync(IDVariant, size, color);

                if (option != null)
                {
                    return Ok(option); // Trả về kết quả dưới dạng chuỗi JSON
                }
                else
                {
                    return NotFound(); // Trả về mã lỗi 404 nếu không tìm thấy
                }
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu cần
                return StatusCode(500); // Trả về mã lỗi 500 nếu có lỗi
            }
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] OptionsCreateVM request)
        {
            if (request.ImagePaths == null)
            {
                return BadRequest("No files received from the upload");
            }
            var result = await _IOptionsService.CreateAsync(request);
            if (result)
            {
                return Ok(new { status = "Success", message = "Options uploaded successfully." });
            }
            else
            {
                return BadRequest(new { status = "Error", message = "There was an error uploading the Options." });
            }
        }
        [HttpPut]
        [Route("update/{ID}")]
        public async Task<IActionResult> UpdateOption(Guid ID, [FromForm] OptionsUpdateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _IOptionsService.UpdateAsync(ID, request);

            if (!result)
            {
                return NotFound("Option not found.");
            }

            return NoContent(); // Hoặc trả về một response phù hợp
        }

        [HttpGet]
        [Route("GetVariantByID/{IDOptions}")]
        public async Task<IActionResult> GetVariantByID(Guid IDOptions)
        {
            var variantdata = await _IOptionsService.GetVariantByID(IDOptions);
            return Ok(variantdata);
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var obj = await _IOptionsService.GetAllAsync();
            return Ok(obj);
        }

        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var obj = await _IOptionsService.GetAllActiveAsync();
            return Ok(obj);
        }

        [HttpGet]
        [Route("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(Guid ID)
        {
            var obj = await _IOptionsService.GetByIDAsync(ID);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

    }
}
