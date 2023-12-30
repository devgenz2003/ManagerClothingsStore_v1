using CHERRY.UI.Models;
using CHERRY.UI.Repositorys._1_Interface;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.UI.Controllers
{
    public class VariantController : Controller
    {
        private readonly IVariantRepository _IVariantRepository;
        private readonly IOptionsRepository _IOptionsRepository;
        private readonly IReviewRepository _IReviewRepository;
        public VariantController(IVariantRepository IVariantRepository, IOptionsRepository IOptionsRepository, IReviewRepository iReviewRepository)
        {
            _IOptionsRepository = IOptionsRepository;
            _IVariantRepository = IVariantRepository;
            _IReviewRepository = iReviewRepository;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productList = await _IVariantRepository.GetAllActiveAsync();

            if (productList == null || !productList.Any())
            {
                return Content("Hiện chưa có cập nhật thêm về sản phẩm");
            }

            return View(productList);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid IDVariant)
        {

            var product = await _IVariantRepository.GetByIDAsync(IDVariant);
            var getoptions = await _IVariantRepository.GetOptionVariantByIDAsync(IDVariant);
            var review = await _IReviewRepository.GetByVariant(IDVariant);
            if (product == null)
            {
                return Content("Sản phẩm không tồn tại.");
            }
            var viewmodels = new CompositeViewModel_Client()
            {
                LstReviewVM = review,
                VariantsVM = product,
                LstOptionsVM = getoptions
            };
            return View(viewmodels);
        }
    }
}
