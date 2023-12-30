using CHERRY.UI.Models;
using CHERRY.UI.Repositorys._1_Interface;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRespository _ICategoryRespository;
        private readonly ICategoriesVariantsRepository _ICategoriesVariantsRepository;
        public CategoryController(ICategoryRespository ICategoryRespository, ICategoriesVariantsRepository iCategoriesVariantsRepository)
        {
            _ICategoryRespository = ICategoryRespository;
            _ICategoriesVariantsRepository = iCategoriesVariantsRepository;
        }
        [HttpGet]
        [Route("category_list")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _ICategoryRespository.GetAllActiveAsync();
                var variant = await _ICategoriesVariantsRepository.GetAllActiveAsync();
                var model = new CompositeViewModel_Client()
                {
                    LstCategoriesVariantsVM = variant,
                    LstCategoryVM = data
                };
                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
