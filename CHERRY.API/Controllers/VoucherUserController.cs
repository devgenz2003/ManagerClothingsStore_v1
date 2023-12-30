using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.Services._2_Implements;
using CHERRY.BUS.ViewModels.VoucherUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherUserController : ControllerBase
    {
        private readonly IVoucherUserService _voucherUserService;

        public VoucherUserController(IVoucherUserService voucherUserService)
        {
            _voucherUserService = voucherUserService;
        }

        // Tạo mới một liên kết VoucherUser
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VoucherUserCreateVM createVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _voucherUserService.CreateAsync(createVM);
            if (!result)
                return BadRequest("Could not create VoucherUser");

            return Ok(); // Hoặc trả về thông tin chi tiết của VoucherUser vừa được tạo
        }

        // Lấy thông tin VoucherUser dựa trên ID
        [HttpGet("{IDVoucher}/{IDUser}")]
        public async Task<IActionResult> GetByID(Guid IDVoucher, string IDUser)
        {
            var voucherUser = await _voucherUserService.GetByIDAsync(IDVoucher, IDUser);
            if (voucherUser == null)
                return NotFound();

            return Ok(voucherUser);
        }

        // Cập nhật thông tin VoucherUser
        [HttpPut("{IDVoucher}/{IDUser}")]
        public async Task<IActionResult> Update(Guid IDVoucher, string IDUser, [FromBody] VoucherUserUpdateVM updateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _voucherUserService.UpdateAsync(IDVoucher, IDUser, updateVM);
            if (!result)
                return BadRequest("Could not update VoucherUser");

            return Ok(); // Hoặc trả về thông tin chi tiết của VoucherUser sau khi cập nhật
        }

        // Xóa VoucherUser
        [HttpDelete("{IDVoucher}/{IDUser}/{IDUserDelete}")]
        public async Task<IActionResult> Delete(Guid IDVoucher, string IDUser, Guid IDUserDelete)
        {
            var result = await _voucherUserService.RemoveAsync(IDVoucher, IDUser, IDUserDelete);
            if (!result)
                return NotFound();

            return Ok();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var classifies = await _voucherUserService.GetAllAsync();
            return Ok(classifies);
        }

        [HttpGet("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var classifies = await _voucherUserService.GetAllActiveAsync();
            return Ok(classifies);
        }
    }
}
