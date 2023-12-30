using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.OrderVariant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderVariantController : ControllerBase
    {
        private readonly IOrderVariantService _IOrderVariantService;

        public OrderVariantController(IOrderVariantService IOrderVariantService)
        {
            _IOrderVariantService = IOrderVariantService;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> AddAsync([FromBody] OrderVariantCreateVM request)
        {
            if (request == null) return BadRequest();
            var result = await _IOrderVariantService.CreateAsync(request);

            return Ok(result);
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var orderDetails = await _IOrderVariantService.GetAllAsync();
            if (orderDetails == null || orderDetails.Count == 0)
            {
                return NoContent();
            }

            return Ok(orderDetails);
        }

        [HttpGet]
        [Route("allactive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var objCollection = await _IOrderVariantService.GetAllActiveAsync();

            return Ok(objCollection);
        }
        [HttpGet]
        [Route("GetByID/{ID}")]
        public async Task<IActionResult> GetByIDAsync(Guid ID)
        {
            var orderDetail = await _IOrderVariantService.GetByIDAsync(ID);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return Ok(orderDetail);
        }
    }
}
