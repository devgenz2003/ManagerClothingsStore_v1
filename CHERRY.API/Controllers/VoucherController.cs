using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Voucher;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var vouchers = await _voucherService.GetAllAsync();
            return Ok(vouchers);
        }
        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var vouchers = await _voucherService.GetAllActiveAsync();
            return Ok(vouchers);
        }
        // Lấy voucher theo ID
        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetVoucher(Guid ID)
        {
            var voucher = await _voucherService.GetByIDAsync(ID);
            if (voucher == null)
                return NotFound();

            return Ok(voucher);
        }

        [HttpGet]
        [Route("GetVoucherByIDUser/{IDUser}")]
        public async Task<IActionResult> GetVoucherByIDUser(string IDUser)
        {
            var datavoucher = await _voucherService.GetVoucherByUser(IDUser);
            return Ok(datavoucher);
        }
        // Tạo voucher mới
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] VoucherCreateVM voucherCreateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _voucherService.CreateAsync(voucherCreateVM);
            if (!created)
                return BadRequest("Could not create voucher");

            return Ok(created);
        }

        // Cập nhật voucher
        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateVoucher(Guid ID, [FromBody] VoucherUpdateVM voucherUpdateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _voucherService.UpdateAsync(ID, voucherUpdateVM);
            if (!updated)
                return BadRequest("Could not update voucher");

            return NoContent();
        }

        // Xóa voucher
        [HttpDelete("{ID}/{IDUserDelete}")]
        public async Task<IActionResult> DeleteVoucher(Guid ID, string IDUserDelete)
        {
            var deleted = await _voucherService.RemoveAsync(ID, IDUserDelete);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
