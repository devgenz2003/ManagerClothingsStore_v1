using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.Services._2_Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionVariantController : ControllerBase
    {
        private readonly IPromotionVariantService _IPromotionVariantService;
        public PromotionVariantController(IPromotionVariantService IPromotionVariantService)
        {
            _IPromotionVariantService = IPromotionVariantService;
        }
        [HttpGet]
        [Route("{IDVariant}/{IDPromotion}")]
        public async Task<IActionResult> GetByID(Guid IDVariant, Guid IDPromotion)
        {
            var history = await _IPromotionVariantService.GetByIDAsync(IDVariant, IDPromotion);
            if (history == null)
                return NotFound();

            return Ok(history);
        }
        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var objCollection = await _IPromotionVariantService.GetAllActiveAsync();

            return Ok(objCollection);
        }

    }
}
