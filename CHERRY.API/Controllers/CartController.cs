using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _ICartService;
        public CartController(ICartService ICartService)
        {
            _ICartService = ICartService;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CartCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _ICartService.CreateAsync(model);
            if (!result)
            {
                return BadRequest(new { Message = "Error creating Cart" });
            }

            return Ok(new { Message = "Cart created successfully" });
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var classifies = await _ICartService.GetAllAsync();
            return Ok(classifies);
        }

        [HttpGet("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var classifies = await _ICartService.GetAllActiveAsync();
            return Ok(classifies);
        }

        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetById(Guid ID)
        {
            var classify = await _ICartService.GetByIDAsync(ID);
            if (classify == null)
            {
                return NotFound();
            }

            return Ok(classify);
        } 
        
        [HttpGet("GetCartByUserIDAsync/{ID_USER}")]
        public async Task<IActionResult> GetCartByUserIDAsync(string ID_USER)
        {
            var classify = await _ICartService.GetCartByUserIDAsync(ID_USER);
            if (classify == null)
            {
                return NotFound();
            }

            return Ok(classify);
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateClassify(Guid ID, [FromBody] CartUpdateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _ICartService.UpdateAsync(ID, request);
            if (!result)
            {
                return BadRequest(new { Message = "Error updating Cart" });
            }

            return Ok(new { Message = "Cart updated successfully" });
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteClassify(Guid ID, [FromQuery] string IDUserdelete)
        {
            var result = await _ICartService.RemoveAsync(ID, IDUserdelete);
            if (!result)
            {
                return BadRequest(new { Message = "Error deleting Cart" });
            }

            return Ok(new { Message = "Cart deleted successfully" });
        }
    }
}
