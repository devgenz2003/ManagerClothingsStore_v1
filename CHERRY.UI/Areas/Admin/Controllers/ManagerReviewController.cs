using CHERRY.UI.Repositorys._1_Interface;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.UI.Areas.Admin.Controllers
{
    public class ManagerReviewController : Controller
    {
        private readonly IReviewRepository _IReviewRepository;
        public ManagerReviewController(IReviewRepository IReviewRepository)
        {
            _IReviewRepository = IReviewRepository;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _IReviewRepository.GetAllActiveAsync();
            return View("~/Areas/Admin/Views/ManagerReview/Index.cshtml", data);
        } 
        
        public async Task<IActionResult> Index_1()
        {
            var data = await _IReviewRepository.GetAllAsync();
            return View("~/Areas/Admin/Views/ManagerReview/Index_1.cshtml", data);
        }

        public async Task<IActionResult> Details(Guid ID)
        {
            var data = await _IReviewRepository.GetByIDAsync(ID);
            return View("~/Areas/Admin/Views/ManagerReview/Details.cshtml", data);
        }
    }
}
