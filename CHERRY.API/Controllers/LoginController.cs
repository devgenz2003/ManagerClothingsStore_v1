using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.User;
using CHERRY.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ICartService _ICartService;
        private readonly UserManager<User> _userManager;
        private readonly ILogin_Service _ILogin_Service;
        public LoginController(ICartService ICartService,ILogin_Service ILogin_Service, UserManager<User> userManager)
        {
            _userManager = userManager;
            _ICartService = ICartService;
            _ILogin_Service = ILogin_Service;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var response = await _ILogin_Service.Login(model);

            if (!response.IsSuccess)
            {
                return Unauthorized(response.Message);
            }

            return Ok(new
            {
                token = response.Token,
                role = response.Roles.FirstOrDefault() // Sử dụng Roles từ phản hồi
            });
        }
    }
}
