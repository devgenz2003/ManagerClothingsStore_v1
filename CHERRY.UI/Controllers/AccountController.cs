using CHERRY.BUS.ViewModels.User;
using CHERRY.UI.Repositorys._1_Interface;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CHERRY.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserResponse _IUserResponse;
        public AccountController(IUserResponse IUserResponse)
        {
            _IUserResponse = IUserResponse;
        }
        [HttpGet]
        public async Task<IActionResult> Account()
        {
            string token = HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);

            if (userID == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var data = await _IUserResponse.GetByIDAsync(userID);
            if (data == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(data);
        }
        private string ExtractUserIDFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken?.Claims != null)
                {
                    var userIDClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
                    if (userIDClaim != null)
                    {
                        return userIDClaim.Value;
                    }
                }
                return null; // Trả về null nếu không tìm thấy ID
            }
            catch (Exception ex)
            {
                // Có thể ghi log lỗi ở đây
                return null; // Trả về null trong trường hợp lỗi
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string ID)
        {
            var data = await _IUserResponse.GetByIDAsync(ID);
            ViewBag.IDUser = ID;
            ViewBag.UserData = data;
            return View();
        }
        [HttpPut]
        public async Task<IActionResult> Edit(string ID, UserUpdateVM request)
        {
            var data = await _IUserResponse.UpdateAsync(ID, request);
            if (data)
            {
                return Content("OK");
            }
            return BadRequest("KO");
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View("/Views/Account/ChangePassword.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string token = HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);
            var result = await _IUserResponse.ChangePasswordAsync(userID, model);
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Đổi mật khẩu thất bại.");
            return View(model);
        }

    }
}
