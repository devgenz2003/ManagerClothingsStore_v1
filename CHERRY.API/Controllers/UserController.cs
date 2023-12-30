using CHERRY.BUS.Services._1_Interface;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.Services._2_Implements;
using CHERRY.BUS.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _IUserService;
        public UserController(IUserService userService)
        {
            _IUserService = userService;
        }

        [HttpPost]
        [Route("{IDUser}/changerole")]
        public async Task<IActionResult> ChangeUserRole(string IDUser, [FromBody] UserRoleChangeRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var result = await _IUserService.ChangeUserRoleAsync(IDUser, request.NewRole);

            if (result)
            {
                return Ok("Role changed successfully");
            }

            return NotFound("User not found or role change failed");
        }

        [HttpPost]
        [Route("{IDUser}/changepassword")]
        public async Task<IActionResult> ChangePassword(string IDUser, [FromBody] ChangePasswordViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var result = await _IUserService.ChangePasswordAsync(IDUser, request);

            if (result)
            {
                return Ok("Changed successfully");
            }

            return NotFound("User not found or change failed");
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var objCollection = await _IUserService.GetAllAsync();

            return Ok(objCollection);
        }

        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var objCollection = await _IUserService.GetAllActiveAsync();

            return Ok(objCollection);
        }


        [HttpGet]
        [Route("GetByID/{ID}")]
        public async Task<IActionResult> GetByIDAsync(string ID)
        {
            var objVM = await _IUserService.GetByIDAsync(ID);


            return Ok(objVM);
        }

        [HttpGet]
        [Route("GetByGmail/{Gmail}")]
        public async Task<IActionResult> GetByGmailAsync(string Gmail)
        {
            var objVM = await _IUserService.GetByGmailAsync(Gmail);
            return Ok(objVM);
        }
        [HttpDelete]
        [Route("{ID}")]
        public async Task<IActionResult> RemoveAsync(string ID, Guid idUserdelete)
        {
            var objDelete = await _IUserService.GetByIDAsync(ID);
            if (objDelete == null) return NotFound();

            var result = await _IUserService.RemoveAsync(ID, idUserdelete);

            return Ok(result);
        }
    }
}
