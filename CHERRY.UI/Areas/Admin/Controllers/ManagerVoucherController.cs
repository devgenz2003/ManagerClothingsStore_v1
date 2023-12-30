using CHERRY.BUS.ViewModels.Voucher;
using CHERRY.UI.Areas.Admin.Models;
using CHERRY.UI.Repositorys._1_Interface;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CHERRY.UI.Areas.Admin.Controllers
{
    public class ManagerVoucherController : Controller
    {
        private readonly IVoucherRepository _IVoucherRepository;
        private readonly IUserResponse _IUserResponse;
        public ManagerVoucherController(IVoucherRepository IVoucherRepository, IUserResponse iUserResponse)
        {
            _IVoucherRepository = IVoucherRepository;
            _IUserResponse = iUserResponse;
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
                    if (userIDClaim != null && Guid.TryParse(userIDClaim.Value, out Guid userID))
                    {
                        return userIDClaim.Value;
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        [HttpGet]
        //[Route("voucher_list")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _IVoucherRepository.GetAllActiveAsync();
                return View("~/Areas/Admin/Views/ManagerVoucher/Index.cshtml", data);
            }
            catch (Exception ex)
            {
                return BadRequest("Chưa có cập nhật gì về voucher");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var Lstuser = await _IUserResponse.GetAllActiveAsync();
            var model = new ModelCompositeShare()
            {
                LstUser = Lstuser
            };
            return View("~/Areas/Admin/Views/ManagerVoucher/Create.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(VoucherCreateVM request)
        {
            try
            {
                var data = await _IVoucherRepository.CreateAsync(request);
                if(data == true)
                {
                    return View("~/Areas/Admin/Views/ManagerVoucher/Index.cshtml", data);
                }
                return BadRequest("Chưa có cập nhật gì về voucher");

            }
            catch (Exception ex)
            {
                return BadRequest("Chưa có cập nhật gì về voucher");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid ID)
        {
            try
            {
                var data = await _IVoucherRepository.GetByIDAsync(ID);
                return View("~/Areas/Admin/Views/ManagerVoucher/Details.cshtml", data);
            }
            catch (Exception ex)
            {
                return BadRequest("Chưa có cập nhật gì về voucher");
            }
        }
    }
}
