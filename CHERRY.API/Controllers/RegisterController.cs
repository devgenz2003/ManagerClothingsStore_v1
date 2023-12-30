using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterServices _registerService;
        public RegisterController(IRegisterServices registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser, string role)
        {
            var result = await _registerService.RegisterAsync(registerUser, role);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
