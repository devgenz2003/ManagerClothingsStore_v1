using CHERRY.BUS.ViewModels.Review;
using CHERRY.UI.Repositorys._1_Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CHERRY.UI.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _IReviewRepository;
        private readonly IVariantRepository _IVariantRepository;
        private readonly IOptionsRepository _IOptionsRepository;
        public ReviewController(IReviewRepository IReviewRepository, IVariantRepository iVariantRepository, IOptionsRepository iOptionsRepository)
        {
            _IReviewRepository = IReviewRepository;
            _IVariantRepository = iVariantRepository;
            _IOptionsRepository = iOptionsRepository;
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
        [Route("review_list")]
        public async Task<IActionResult> Index(Guid IDOptions)
        {
            var variant = await _IOptionsRepository.GetVariantByID(IDOptions);
            var reviewdata = await _IReviewRepository.GetByVariant(variant);
            if (reviewdata != null)
            {
                return View(reviewdata);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create(Guid IDOptions, Guid IDOrderVariant)
        {
            string token = HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);
            ViewBag.IDUser = userID;
            var IDVariant = await _IOptionsRepository.GetVariantByID(IDOptions);

            var model = new ReviewCreateVM()
            {
                ID = Guid.NewGuid(),
                IDVariant = IDVariant,
                IDOrderVariant = IDOrderVariant,
            };

            return View("~/Views/Review/Create.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ." });
            }
            try
            {
                return Json(new { success = true, message = "Thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Xảy ra lỗi khi xử lý." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid ID)
        {
            try
            {
                var data = await _IReviewRepository.GetByIDAsync(ID);
                return View(data);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
