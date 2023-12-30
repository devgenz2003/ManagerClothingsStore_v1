using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.Services._2_Implements;
using CHERRY.BUS.ViewModels.VoucherHistory;
using CHERRY.BUS.ViewModels.VoucherUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherHistoryController : ControllerBase
    {
        private readonly IVoucherHistoryService _voucherHistoryService;

        public VoucherHistoryController(IVoucherHistoryService voucherHistoryService)
        {
            _voucherHistoryService = voucherHistoryService;
        }

        // Lấy thông tin lịch sử của một voucher cụ thể dựa trên ID
        [HttpGet("{IDVoucher}")]
        public async Task<IActionResult> GetHistoryByVoucherId(Guid IDVoucher)
        {
            var history = await _voucherHistoryService.GetHistoryByVoucherIdAsync(IDVoucher);
            if (history == null)
                return NotFound();

            return Ok(history);
        }

        // Lấy thông tin chi tiết lịch sử dựa trên IDVoucher và IDUser
        [HttpGet("{IDVoucher}/{IDOrder}")]
        public async Task<IActionResult> GetByID(Guid IDVoucher, Guid IDOrder)
        {
            var history = await _voucherHistoryService.GetByIDAsync(IDVoucher, IDOrder);
            if (history == null)
                return NotFound();

            return Ok(history);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VoucherHistoryCreateVM createVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _voucherHistoryService.CreateAsync(createVM);
            if (!result)
                return BadRequest("Could not create VoucherUser");

            return Ok(); // Hoặc trả về thông tin chi tiết của VoucherUser vừa được tạo
        }

        // Cập nhật thông tin VoucherUser
        [HttpPut("{IDVoucher}/{IDOrder}")]
        public async Task<IActionResult> Update(Guid IDVoucher, Guid IDOrder, [FromBody] VoucherHistoryUpdateVM updateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _voucherHistoryService.UpdateAsync(IDVoucher, IDOrder, updateVM);
            if (!result)
                return BadRequest("Could not update VoucherUser");

            return Ok(); // Hoặc trả về thông tin chi tiết của VoucherUser sau khi cập nhật
        }
        // Xóa VoucherUser
        [HttpDelete("{IDVoucher}/{IDOrder}/{IDUserDelete}")]
        public async Task<IActionResult> Delete(Guid IDVoucher, Guid IDOrder, Guid IDUserDelete)
        {
            var result = await _voucherHistoryService.RemoveAsync(IDVoucher, IDOrder, IDUserDelete);
            if (!result)
                return NotFound();

            return Ok();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var classifies = await _voucherHistoryService.GetAllAsync();
            return Ok(classifies);
        }

        [HttpGet("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var classifies = await _voucherHistoryService.GetAllActiveAsync();
            return Ok(classifies);
        }
    }
}
