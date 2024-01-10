using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.Services._2_Implements;
using CHERRY.BUS.ViewModels.User;
using CHERRY.BUS.ViewModels.Variants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CHERRY.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VariantsController : ControllerBase
    {
        private readonly IVariantsService _IVariantsService;
        private readonly IMemoryCache _cache;
        public VariantsController(IVariantsService IVariantsService, IMemoryCache cache)
        {
            _IVariantsService = IVariantsService ?? throw new ArgumentNullException(nameof(IVariantsService));
            _cache = cache;
        }
        [HttpPut]
        [Route("Update/{ID}")]
        public async Task<IActionResult> Update(Guid ID, [FromForm] VariantsUpdateVM request)
        {
            try
            {
                var user = await _IVariantsService.GetByIDAsync(ID);

                if (user != null)
                {
                    var data = await _IVariantsService.UpdateAsync(ID, request);
                    return Ok(data);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi không thể cập nhật: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] VariantsCreateVM request)
        {
            if (request.ImagePaths == null)
            {
                return BadRequest("No files received from the upload");
            }
            var result = await _IVariantsService.CreateAsync(request);
            if (result)
            {
                return Ok(new { status = "Success", message = "variants uploaded successfully." });
            }
            else
            {
                return BadRequest(new { status = "Error", message = "There was an error uploading the variants." });
            }
        }
        [HttpGet]
        [Route("GetByID/{IDVariant}")]
        public async Task<IActionResult> GetByID(Guid IDVariant)
        {
            var detail = await _IVariantsService.GetByIDAsync(IDVariant);
            if (detail == null)
            {
                return NotFound();
            }
            return Ok(detail);
        }
        [HttpGet]
        [Route("GetVariantByID/{IDVariant}")]
        public async Task<IActionResult> GetVariantByID(Guid IDVariant)
        {
            var detail = await _IVariantsService.GetOptionVariantByIDAsync(IDVariant);
            if (detail == null)
            {
                return NotFound();
            }
            return Ok(detail);
        }
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var objCollection = await _IVariantsService.GetAllAsync();

            return Ok(objCollection);
        }
        [HttpGet]
        [Route("allactive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var objCollection = await _IVariantsService.GetAllActiveAsync();

            return Ok(objCollection);
        }
    }
}
