using CHERRY.BUS.ViewModels.User;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.UI.Areas.Admin.Models;
using CHERRY.UI.Models;
using CHERRY.UI.Repositorys._1_Interface;
using CHERRY.UI.Repositorys._2_Implement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CHERRY.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVariantRepository _IVariantRepository;
        private readonly IOptionsRepository _IOptionsRepository;
        private readonly ICategoryRespository _ICategoryRespository;
        private readonly IReviewRepository _IReviewRepository;
        private readonly IPromotionVariantRepository _IPromotionVariantRepository;
        private readonly CHERRY_DBCONTEXT _dbcontext;
        public HomeController(ILogger<HomeController> logger,
            IVariantRepository IVariantRepository, IOptionsRepository IOptionsRepository, 
            IReviewRepository iReviewRepository, CHERRY_DBCONTEXT CHERRY_DBCONTEXT,
            IPromotionVariantRepository IPromotionVariantRepository, ICategoryRespository iCategoryRespository)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _IPromotionVariantRepository = IPromotionVariantRepository;
            _IOptionsRepository = IOptionsRepository;
            _IVariantRepository = IVariantRepository;
            _IReviewRepository = iReviewRepository;
            _logger = logger;
            _ICategoryRespository = iCategoryRespository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productList = await _IVariantRepository.GetAllActiveAsync();
            var cataegory = await _ICategoryRespository.GetAllActiveAsync();
            var variantWithPromotion = await _IPromotionVariantRepository.GetAllActiveAsync();

            var model = new ModelCompositeShare()
            {
                LstVariantsVM = productList,
                LstCategoryVM = cataegory,
                LstPromotionVariantsVM = variantWithPromotion
            };

            return View(model);
        }
        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser, string role)
        {
            return View(); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
