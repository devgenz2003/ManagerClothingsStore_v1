using CHERRY.BUS.ViewModels.Options;
using CHERRY.UI.Repositorys._1_Interface;
using CHERRY.UI.Repositorys._2_Implement;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        [Route("createoption")]
        public async Task<IActionResult> Create()
        {
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
            //if (request == null)
            //{
            //    throw new ArgumentNullException(nameof(request));
            //}
            //var data = await _IOptionsRepository.CreateAsync(request);
            //if (data)
            //{
            //    return RedirectToAction("Index", "ManagerOptions");
            //}
            //return View(request);
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
            var variant = await _VariantRepository.GetAllAsync();
            ViewBag.Variant = variant;
            var dataold = await _IOptionsRepository.GetByIDAsync(ID);
            var updateVM = new OptionsUpdateVM
            {
                IDColor = dataold.IDColor,
                IDSizes = dataold.IDSizes,
                IDVariant = dataold.IDVariant,
                ColorName = dataold.ColorName,
                SizesName = dataold.SizeName,
                CostPrice = dataold.CostPrice,
                RetailPrice = dataold.RetailPrice,
                DiscountedPrice = dataold.DiscountedPrice,
                Description = dataold.Description,
                //ImageURL = dataold.ImageURL,
                StockQuantity = dataold.StockQuantity,
                Status = dataold.Status,
            };
            return View("~/Areas/Admin/Views/ManagerOptions/Edit.cshtml", updateVM);
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
