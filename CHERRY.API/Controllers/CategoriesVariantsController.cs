using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.CategoriesVariants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesVariantsController : ControllerBase
    {
        private readonly ICategoriesVariantsService _ICategoriesVariantsService;

        public CategoriesVariantsController(ICategoriesVariantsService ICategoriesVariantsService)
        {
            _ICategoriesVariantsService = ICategoriesVariantsService;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> AddAsync([FromBody] CategoriesVariantsCreateVM request)
        {
            if (request == null) return BadRequest();
            var result = await _ICategoriesVariantsService.CreateAsync(request);

            return Ok(result);
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var objCollection = await _ICategoriesVariantsService.GetAllAsync();

            return Ok(objCollection);
        }

        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var objCollection = await _ICategoriesVariantsService.GetAllActiveAsync();

            return Ok(objCollection);
        }
        [HttpGet("priceRange")]
        public async Task<ActionResult<List<CategoriesVariantsVM>>> GetVariantsWithinPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var variantsInRange = await _ICategoriesVariantsService.GetMinMaxRetails(minPrice, maxPrice);
                return variantsInRange;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{IDCategory}/minMaxPrices")]
        public async Task<ActionResult<Tuple<decimal, decimal>>> GetMinMaxPricesForCategory(Guid IDCategory)
        {
            try
            {
                var prices = await _ICategoriesVariantsService.GetMinMaxPricesForCategory(IDCategory);
                return prices;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("{IDVariant}/{IDCategory}")]
        public async Task<IActionResult> GetByIdAsync(Guid IDVariant, Guid IDCategory)
        {
            var objVM = await _ICategoriesVariantsService.GetByIDAsync(IDVariant, IDCategory);

            return Ok(objVM);
        }

    }
}
