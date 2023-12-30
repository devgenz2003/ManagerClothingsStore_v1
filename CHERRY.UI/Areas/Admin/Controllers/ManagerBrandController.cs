using CHERRY.BUS.ViewModels.Brand;
using CHERRY.UI.Repositorys._1_Interface;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CHERRY.UI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ManagerBrandController : Controller
    {
        private readonly IBrandRepository _IBrandRepository;
        public ManagerBrandController(IBrandRepository IBrandRepository)
        {
            _IBrandRepository = IBrandRepository;
        }
        [HttpGet]
        [Route("brandlist")]
        public async Task<IActionResult> Index()
        {
            if(_IBrandRepository == null)
            {
                return NotFound();
            }
            var data = await _IBrandRepository.GetAllActiveAsync();
            return View(data);
        }
        [HttpGet]
        [Route("brandcreate")]
        public async Task<IActionResult> Create()
        {
            return View("~/Areas/Admin/Views/ManagerBrand/Create.cshtml");
        }
        [HttpPost]
        [Route("brandcreate")]
        public async Task<IActionResult> Create(BrandCreateVM request)
        {
            string token = HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);
            try
            {
                var productcreate = await _IBrandRepository.CreateAsync(request);
                request.CreateBy = userID;
                if (productcreate)
                {
                    return RedirectToAction("Index", "ManagerBrand");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return View("ErrorView", ex); // Trả về một view lỗi với thông tin chi tiết
            }
        }
        [HttpGet]
        [Route("updatebrand")]
        public async Task<IActionResult> Edit(Guid ID)
        {
            var dataold = await _IBrandRepository.GetByIDAsync(ID);
            var updateVM = new BrandUpdateVM
            {
                Name = dataold.Name,
                Description = dataold.Description,
                Status = dataold.Status,
            };
            return View("~/Areas/Admin/Views/ManagerBrand/Edit.cshtml", updateVM);
        }
        [HttpPut]
        [Route("updatebrand/{ID}")]
        public async Task<IActionResult> Edit(Guid ID, BrandUpdateVM request)
        {
            if (request == null)
            {
                return BadRequest("Request data is null.");
            }

            var success = await _IBrandRepository.UpdateAsync(ID, request);
            if (!success)
            {
                return BadRequest("Unable to update the specified option.");
            }

            return RedirectToAction("Index", "ManagerBrand");
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
    }
}
