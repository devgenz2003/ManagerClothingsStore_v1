using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.CartProductVariants;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartProductVariantsController : ControllerBase
    {
        private readonly ICartProductVariantsService _ICartClassifyItemService;

        public CartProductVariantsController(ICartProductVariantsService ICartClassifyItemService)
        {
            _ICartClassifyItemService = ICartClassifyItemService;
        }

        [HttpPost]
        [Route("AddToCart")]
        public async Task<IActionResult> AddAsync([FromBody] CartProductVariantsCreateVM request)
        {
            if (request == null) return BadRequest();
            var result = await _ICartClassifyItemService.CreateAsync(request);

            return Ok(result);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var objCollection = await _ICartClassifyItemService.GetAllAsync();

            return Ok(objCollection);
        }

        [HttpGet]
        [Route("allactive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var objCollection = await _ICartClassifyItemService.GetAllActiveAsync();

            return Ok(objCollection);
        }

        [HttpGet]
        [Route("{ID_Cart}/{ID_ProductVariants}")]
        public async Task<IActionResult> GetByIdAsync(Guid ID_Cart, Guid? IDVariants)
        {
            var objVM = await _ICartClassifyItemService.GetByIDAsync(ID_Cart, IDVariants);

            return Ok(objVM);
        }

        [HttpDelete]
        [Route("{ID_Cart}/{ID_ProductVariants}/{idUserdelete}")]
        public async Task<IActionResult> RemoveAsync(Guid ID_Cart, Guid? IDVariants, Guid idUserdelete)
        {
            var objDelete = await _ICartClassifyItemService.GetByIDAsync(ID_Cart, IDVariants);
            if (objDelete == null) return NotFound();

            var result = await _ICartClassifyItemService.RemoveAsync(ID_Cart, IDVariants, idUserdelete);

            return Ok(result);
        }

        [HttpPut]
        [Route("{ID_Cart}/{ID_ProductVariants}")]
        public async Task<IActionResult> UpdateAsync(Guid ID_Cart, Guid? IDVariants, [FromBody] CartProductVariantsUpdateVM request)
        {
            if (request == null) return BadRequest();
            var result = await _ICartClassifyItemService.UpdateAsync(ID_Cart, IDVariants, request);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllByCartIDAsync/{ID_Cart}")]
        public async Task<IActionResult> GetAllByCartIDAsync(Guid ID_Cart)
        {
            var objVM = await _ICartClassifyItemService.GetAllByCartIDAsync(ID_Cart);

            return Ok(objVM);
        }
    }
}
