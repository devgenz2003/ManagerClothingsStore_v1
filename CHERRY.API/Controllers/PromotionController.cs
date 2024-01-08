using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.BUS.ViewModels.PromotionVariants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }
        [HttpGet]
        [Route("{ID}/Variants")]
        public async Task<ActionResult<List<PromotionVariantsVM>>> GetVariantsInPromotion(Guid ID)
        {
            try
            {
                var variants = await _promotionService.GetVariantsInPromotionAsync(ID);
                if (variants == null )
                {
                    return NotFound(); // Return 404 if no variants found
                }
                return Ok(variants); // Return 200 with the variants
            }
            catch (Exception ex)
            {
                // Log the exception...
                return StatusCode(500, "Internal server error"); // Return 500 if there's an error
            }
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var promotions = await _promotionService.GetAllAsync();
            return Ok(promotions);
        }

        [HttpGet]
        [Route("getallactive")]

        public async Task<IActionResult> GetAllActive()
        {
            var promotions = await _promotionService.GetAllActiveAsync();
            return Ok(promotions);
        }

        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetById(Guid ID)
        {
            var promotion = await _promotionService.GetByIDAsync(ID);
            return promotion != null ? Ok(promotion) : NotFound();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(PromotionCreateVM request)
        {
            var result = await _promotionService.CreateAsync(request);
            return result ? Ok() : BadRequest();
        }

        [HttpPut]
        [Route("Edit_promotion/{ID}")]
        public async Task<IActionResult> Update(Guid ID, PromotionUpdateVM request)
        {
            var result = await _promotionService.UpdateAsync(ID, request);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> Delete(Guid ID)
        {
            var result = await _promotionService.RemoveAsync(ID, User.Identity?.Name);
            return result ? Ok() : NotFound();
        }

        [HttpPost("activate/{ID}")]
        public async Task<IActionResult> Activate(Guid ID)
        {
            var result = await _promotionService.ActivatePromotionAsync(ID);
            return result ? Ok() : NotFound();
        }

        [HttpPost("deactivate/{ID}")]
        public async Task<IActionResult> Deactivate(Guid ID)
        {
            var result = await _promotionService.DeactivatePromotionAsync(ID);
            return result ? Ok() : NotFound();
        }
        // Trong PromotionController

        [HttpPost("validate/{ID}")]
        public async Task<IActionResult> Validate(Guid ID)
        {
            var result = await _promotionService.ValidatePromotionAsync(ID);
            return result ? Ok() : NotFound();
        }
    }
}
