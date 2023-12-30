using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.UI.Areas.Admin.Models;
using CHERRY.UI.Repositorys._1_Interface;
using CHERRY.UI.Repositorys._2_Implement;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.UI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ManagerPromotionController : Controller
    {
        private readonly IPromotionRepository _IPromotionRepository;
        private readonly IVariantRepository _IVariantRepository;
        public ManagerPromotionController(IPromotionRepository IPromotionRepository, IVariantRepository IVariantRepository)
        {
            _IVariantRepository = IVariantRepository;
            _IPromotionRepository = IPromotionRepository;
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
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var data = await _IPromotionRepository.CreateAsync(request);
            if (data)
            {
                return View("~/Areas/Admin/Views/ManagerPromotion/Index.cshtml", data);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("details_promotion")]
        public async Task<IActionResult> Details(Guid ID)
        {
            var data = await _IPromotionRepository.GetByIDAsync(ID);
            var variant = await _IPromotionRepository.GetVariantsInPromotionAsync(ID);
            var model = new ModelCompositeShare()
            {
                LstPromotionVariantsVM = variant,
                PromotionVM = data
            };
            return View("~/Areas/Admin/Views/ManagerPromotion/Details.cshtml", model);
        }
    }
}
