using CHERRY.BUS.ViewModels.Options;
using CHERRY.UI.Repositorys._1_Interface;
using CHERRY.UI.Repositorys._2_Implement;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CHERRY.UI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ManagerOptionsController : Controller
    {
        private readonly IOptionsRepository _IOptionsRepository;
        private readonly IVariantRepository _VariantRepository;
        private readonly IColorsRepository _ColorsRepository;
        private readonly ISizesRepository _SizesRepository;
        public ManagerOptionsController(IOptionsRepository IOptionsRepository, IVariantRepository variantRepository, IColorsRepository colorsRepository, ISizesRepository ISizesRepository)
        {
            _IOptionsRepository = IOptionsRepository;
            _VariantRepository = variantRepository;
            _ColorsRepository = colorsRepository;
            _SizesRepository = ISizesRepository;

        }
        [HttpGet]
        [Route("optionlist")]
        public async Task<IActionResult> Index()
        {
            var data = await _IOptionsRepository.GetAllActiveAsync();
            return View("~/Areas/Admin/Views/ManagerOptions/Index.cshtml", data);
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
        [Route("createoption")]
        public async Task<IActionResult> Create()
        {
            string token = HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);
            ViewBag.IDUser = userID;

            var variant = await _VariantRepository.GetAllActiveAsync();
            ViewBag.Variant = variant;
            var color = await _ColorsRepository.GetAllActiveAsync();
            ViewBag.Color = color;
            var size = await _SizesRepository.GetAllActiveAsync();
            ViewBag.Size = size;
            return View("~/Areas/Admin/Views/ManagerOptions/Create.cshtml");
        }
        [HttpPost]
        [Route("createoption")]
        public async Task<IActionResult> Create(OptionsCreateVM request)
        {
            return View();
        }
        [HttpGet]
        [Route("detailsoptions")]
        public async Task<IActionResult> Details(Guid ID)
        {
            try
            {
                var data = await _IOptionsRepository.GetByIDAsync(ID);
                return View("~/Areas/Admin/Views/ManagerOptions/Details.cshtml", data);
            }
            catch (Exception ex)
            {
                return BadRequest("Chưa có cập nhật gì về options");
            }
        }
        [HttpGet]
        [Route("update/{ID}")]
        public async Task<IActionResult> Edit(Guid ID)
        {            
            var color = await _ColorsRepository.GetAllActiveAsync();
            ViewBag.Color = color;
            var size = await _SizesRepository.GetAllActiveAsync();
            ViewBag.Size = size;
            string token = HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);
            ViewBag.IDUser = userID;
            ViewBag.IDOptions = ID;
            var variant = await _VariantRepository.GetAllAsync();
            ViewBag.Variant = variant;
            var dataold = await _IOptionsRepository.GetByIDAsync(ID);
            ViewBag.Options = dataold;
            ViewBag.IDVariant = dataold.IDVariant;
            return View("~/Areas/Admin/Views/ManagerOptions/Edit.cshtml");
        }
        [HttpPut]
        [Route("update/{ID}")]
        public async Task<IActionResult> Edit(Guid ID, OptionsUpdateVM request)
        {
            if (request == null)
            {
                return BadRequest("Request data is null.");
            }

            var success = await _IOptionsRepository.UpdateAsync(ID, request);
            if (!success)
            {
                return BadRequest("Unable to update the specified option.");
            }

            return RedirectToAction("Index", "ManagerOptions");
        }
    }
}
