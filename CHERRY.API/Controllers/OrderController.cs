using CHERRY.BUS.Services._1_Interface;
using CHERRY.BUS.Services._2_Implements;
using CHERRY.BUS.ViewModels.Order;
using CHERRY.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService IOrderService)
        {
            _orderService = IOrderService;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> AddAsync([FromBody] OrderCreateVM request)
        {
            if (request == null) return BadRequest();
            var result = await _orderService.CreateAsync(request);

            return Ok(result);
        }
        [HttpPatch("{id}/confirm")]
        public async Task<IActionResult> ConfirmOrder(Guid id)
        {
            var isConfirmed = await _orderService.ConfirmOrderAsync(id, true);

            if (isConfirmed)
            {
                return Ok("Order confirmed successfully.");
            }
            else
            {
                return NotFound("Order not found or unable to confirm.");
            }
        }
        [HttpGet]
        [Route("GetOrderDetailsByID/{ID_Order}")]
        public async Task<IActionResult> GetOrderDetailsByID(Guid ID_Order)
        {
            var order = await _orderService.GetOrderVariantByIDAsync(ID_Order);
            if (order == null) return NotFound();
            return Ok(order);
        }
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var objCollection = await _orderService.GetAllAsync();

            return Ok(objCollection);
        }
        [HttpGet]
        [Route("allactive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var objCollection = await _orderService.GetAllActiveAsync();

            return Ok(objCollection);
        }
        [HttpGet("customer/{ID_User}")]
        public async Task<IActionResult> GetByCustomerID(string ID_User)
        {
            var orders = await _orderService.GetByCustomerIDAsync(ID_User);
            if (orders == null) return NotFound();
            return Ok(orders);
        }
        [HttpGet("date")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetByDateRangeAsync(startDate, endDate);
            if (orders == null) return NotFound();
            return Ok(orders);
        }
        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(Guid ID)
        {
            var order = await _orderService.GetByIDAsync(ID);
            if (order == null) return NotFound();
            return Ok(order);
        }
        [HttpGet("{status}")]
        public async Task<IActionResult> GetByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetByStatusAsync(status);
            if (orders == null) return NotFound();
            return Ok(orders);
        }
        [HttpPut("MarkAsCancelled/{ID_Order}")]
        public async Task<IActionResult> MarkAsCancelled(Guid ID_Order)
        {
            var result = await _orderService.MarkAsCancelledAsync(ID_Order);
            if (!result) return BadRequest();
            return Ok();
        }
        [HttpPut("MarkAsDelivered/{ID_Order}")]
        public async Task<IActionResult> MarkAsDelivered(Guid ID_Order)
        {
            var result = await _orderService.MarkAsDeliveredAsync(ID_Order);
            if (!result) return BadRequest();
            return Ok();
        }
        [HttpPut("MarkAsReturned/{ID_Order}")]
        public async Task<IActionResult> MarkAsReturned(Guid ID_Order)
        {
            var result = await _orderService.MarkAsReturnedAsync(ID_Order);
            if (!result) return BadRequest();
            return Ok();
        }
        [HttpPut("MarkAsShipped/{ID_Order}")]
        public async Task<IActionResult> MarkAsShipped(Guid ID_Order)
        {
            var result = await _orderService.MarkAsShippedAsync(ID_Order);
            if (!result) return BadRequest();
            return Ok();
        }
        [HttpDelete("{ID}/{IDUserdelete}")]
        public async Task<IActionResult> Remove(Guid ID, [FromQuery] string IDUserdelete)
        {
            var result = await _orderService.RemoveAsync(ID, IDUserdelete);
            if (!result) return BadRequest();
            return Ok();
        }
        [HttpPut("{ID}")]
        public async Task<IActionResult> Update(Guid ID, [FromBody] OrderUpdateVM request)
        {
            if (request == null) return BadRequest("Request is null.");
            var result = await _orderService.UpdateAsync(ID, request);
            if (!result) return BadRequest("Could not update order.");
            return Ok("Order updated successfully.");
        }
    }
}
