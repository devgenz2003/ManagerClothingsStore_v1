using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.UI.Areas.Admin.Models;
using CHERRY.UI.Repositorys._1_Interface;
using CHERRY.UI.Repositorys._2_Implement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CHERRY.UI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ManagerPromotionController : Controller
    {
        private readonly IPromotionRepository _IPromotionRepository;
        private readonly IOptionsRepository _IOptionsRepository;
        private readonly IVariantRepository _IVariantRepository;
        private readonly CHERRY_DBCONTEXT _dbcontext;
        public ManagerPromotionController(IPromotionRepository IPromotionRepository,
            IVariantRepository IVariantRepository, CHERRY_DBCONTEXT CHERRY_DBCONTEXT, IOptionsRepository iOptionsRepository)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _IVariantRepository = IVariantRepository;
            _IPromotionRepository = IPromotionRepository;
            _IOptionsRepository = iOptionsRepository;
        }
        [HttpGet]
        [Route("promotion_list")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _IPromotionRepository.GetAllAsync();
                return View("~/Areas/Admin/Views/ManagerPromotion/Index.cshtml", data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("create_promotion")]
        public async Task<IActionResult> Create()
        {
            var variant = await _IVariantRepository.GetAllActiveAsync();
            var model = new ModelCompositeShare()
            {
                LstVariantsVM = variant
            };
            return View("~/Areas/Admin/Views/ManagerPromotion/Create.cshtml", model);
        }
        [HttpPost]
        [Route("create_promotion")]
        public async Task<IActionResult> Create(PromotionCreateVM request)
        {            
            return View();
        }
        [HttpGet]
        [Route("details_promotion")]
        public async Task<IActionResult> Details(Guid ID)
        {
            var data = await _IPromotionRepository.GetByIDAsync(ID);
            var startDate = data.StartDate; 
            var endDate = data.EndDate;
            var totalProductsSold = await _dbcontext.OrderVariant
                  .Include(od => od.Order)
                  .Where(od => od.Order.CreateDate >= startDate && od.Order.CreateDate <= endDate)
                  .SumAsync(od => od.Quantity);
            ViewBag.TotalProductsSold = totalProductsSold;

            var totalSales = await _dbcontext.Order
                .Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate)
                .SumAsync(o => o.TotalAmount);
            ViewBag.TotalSales = totalSales;

            var variant = await _IPromotionRepository.GetVariantsInPromotionAsync(ID);

            var model = new ModelCompositeShare()
            {
                LstPromotionVariantsVM = variant,
                PromotionVM = data
            };
            return View("~/Areas/Admin/Views/ManagerPromotion/Details.cshtml", model);
        }
        public decimal CalculateTotalProfit(List<OptionsVM> optionsList)
        {
            decimal totalProfit = 0;

            foreach (var option in optionsList)
            {
                decimal cost = option.CostPrice; // Giá nhập
                decimal retailPrice = option.RetailPrice; // Giá bán
                decimal discount = option.DiscountedPrice.Value; // Giảm giá

                decimal profitPerOption = (retailPrice - discount) - cost;
                totalProfit += profitPerOption;
            }

            return totalProfit;
        }

        [HttpGet]
        [Route("edit_promotions")]
        public async Task<IActionResult> Edit(Guid ID)
        {
            var variant = await _IVariantRepository.GetAllActiveAsync();
            string token = HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);
            ViewBag.IDUser = userID;
            ViewBag.ID = ID;

            var promotion = await _IPromotionRepository.GetByIDAsync(ID);
            ViewBag.Promotions = promotion;
            var model = new ModelCompositeShare()
            {
                PromotionVM = promotion,
                LstVariantsVM = variant
            };
            return View("~/Areas/Admin/Views/ManagerPromotion/Edit.cshtml", model);
        }
        [HttpPut]
        [Route("edit_promotions")]
        public async Task<IActionResult> Edit()
        {
            return View();
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
