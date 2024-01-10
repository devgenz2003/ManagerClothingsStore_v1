using CHERRY.BUS.Services._1_Interface;
using CHERRY.BUS.ViewModels.Variants;
using CHERRY.UI.Areas.Admin.Models;
using CHERRY.UI.Models;
using CHERRY.UI.Repositorys._1_Interface;
using CHERRY.Utilities;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CHERRY.UI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ManagerVariantController : Controller
    {
        private readonly IReviewRepository _IReviewRepository;
        private readonly IOptionsRepository _IOptionsRepository;
        private readonly IColorsRepository _ColorsRepository;
        private readonly ISizesRepository _SizesRepository;
        private readonly IVariantRepository _IVariantRepository;
        private readonly ICategoryRespository _ICategoryRespository;
        private readonly IBrandRepository _IBrandRepository;
        private readonly IMaterialRepository _IMaterialRepository;
        private readonly IMediaAssetsRepository _IMediaAssetsRepository;
        private readonly HttpClient _httpClient;
        public ManagerVariantController(IBrandRepository IBrandRepository, IMaterialRepository IMaterialRepository,
            ICategoryRespository ICategoryRespository, IVariantRepository IVariantRepository, HttpClient httpClient,
            IMediaAssetsRepository IMediaAssetsRepository, IOptionsRepository IOptionsRepository, IColorsRepository colorsRepository,
            ISizesRepository ISizesRepository, IReviewRepository IReviewRepository
            )
        {
            _IOptionsRepository = IOptionsRepository;
            _ColorsRepository = colorsRepository;
            _SizesRepository = ISizesRepository;
            _IReviewRepository = IReviewRepository;
            _ICategoryRespository = ICategoryRespository;
            _IMediaAssetsRepository = IMediaAssetsRepository;
            _IVariantRepository = IVariantRepository;
            _httpClient = httpClient;
            _IBrandRepository = IBrandRepository;
            _IMaterialRepository = IMaterialRepository;
        }
        [HttpGet]
        [Route("ExportVariantToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var productList = await _IVariantRepository.GetAllActiveAsync();

            using (var workbook = new XLWorkbook())
            {
                var variantsWorksheet = workbook.Worksheets.Add("Variants");
                var optionsWorksheet = workbook.Worksheets.Add("Options");

                // Thiết lập tiêu đề cho sheet "Variants"
                variantsWorksheet.Cell(1, 1).Value = "ID";
                variantsWorksheet.Cell(1, 2).Value = "VariantName";
                variantsWorksheet.Cell(1, 3).Value = "BrandName";
                variantsWorksheet.Cell(1, 4).Value = "MaterialName";
                variantsWorksheet.Cell(1, 5).Value = "Minprice";
                variantsWorksheet.Cell(1, 6).Value = "Maxprice";
                variantsWorksheet.Cell(1, 7).Value = "TotalOptions";
                variantsWorksheet.Cell(1, 8).Value = "TotalQuantity";
                variantsWorksheet.Cell(1, 9).Value = "Description";
                variantsWorksheet.Cell(1, 10).Value = "Images";

                // Thiết lập tiêu đề cho sheet "Options"
                optionsWorksheet.Cell(1, 1).Value = "IDVariant";
                optionsWorksheet.Cell(1, 2).Value = "CostPrice";
                optionsWorksheet.Cell(1, 3).Value = "RetailPrice";
                optionsWorksheet.Cell(1, 4).Value = "DiscountedPrice";
                optionsWorksheet.Cell(1, 5).Value = "StockQuantity";
                optionsWorksheet.Cell(1, 6).Value = "Description";
                optionsWorksheet.Cell(1, 7).Value = "ColorName";
                optionsWorksheet.Cell(1, 8).Value = "SizeName";
                optionsWorksheet.Cell(1, 9).Value = "ImageURL";
                optionsWorksheet.Cell(1, 10).Value = "Status";

                int variantCurrentRow = 2;
                int optionCurrentRow = 2;

                foreach (var product in productList)
                {
                    // Ghi thông tin sản phẩm vào sheet "Variants"
                    variantsWorksheet.Cell(variantCurrentRow, 1).Value = product.ID.ToString();
                    variantsWorksheet.Cell(variantCurrentRow, 2).Value = product.VariantName;
                    variantsWorksheet.Cell(variantCurrentRow, 3).Value = product.BrandName;
                    variantsWorksheet.Cell(variantCurrentRow, 4).Value = product.MaterialName;
                    variantsWorksheet.Cell(variantCurrentRow, 5).Value = Currency.FormatCurrency(product.Minprice.ToString()) + "đ";
                    variantsWorksheet.Cell(variantCurrentRow, 6).Value = Currency.FormatCurrency(product.Maxprice.ToString()) + "đ";
                    variantsWorksheet.Cell(variantCurrentRow, 7).Value = product.TotalOptions;
                    variantsWorksheet.Cell(variantCurrentRow, 8).Value = product.TotalQuantity;
                    variantsWorksheet.Cell(variantCurrentRow, 9).Value = product.Description;
                    if (product.ImagePaths != null && product.ImagePaths.Any())
                    {
                        for (int i = 0; i < product.ImagePaths.Count; i++)
                        {
                            var cell = variantsWorksheet.Cell(variantCurrentRow, 10);
                            cell.Value = product.ImagePaths[i];
                            cell.Style.Font.FontColor = XLColor.Blue;
                            cell.Style.Font.Underline = XLFontUnderlineValues.Single;

                            variantCurrentRow++;
                        }
                    }
                    // Lấy và ghi thông tin tùy chọn vào sheet "Options"
                    var options = await _IVariantRepository.GetOptionVariantByIDAsync(product.ID);
                    if (options != null)
                    {
                        foreach (var option in options)
                        {
                            optionsWorksheet.Cell(optionCurrentRow, 1).Value = option.IDVariant.ToString();
                            optionsWorksheet.Cell(optionCurrentRow, 2).Value = Currency.FormatCurrency(option.CostPrice.ToString()) + "đ";
                            optionsWorksheet.Cell(optionCurrentRow, 3).Value = Currency.FormatCurrency(option.RetailPrice.ToString()) + "đ";
                            optionsWorksheet.Cell(optionCurrentRow, 4).Value = Currency.FormatCurrency(option.DiscountedPrice.ToString()) + "đ";
                            optionsWorksheet.Cell(optionCurrentRow, 5).Value = option.StockQuantity;
                            optionsWorksheet.Cell(optionCurrentRow, 6).Value = option.ColorName;
                            optionsWorksheet.Cell(optionCurrentRow, 7).Value = option.SizeName;
                            if (!string.IsNullOrEmpty(option.ImageURL))
                            {
                                var cell = optionsWorksheet.Cell(optionCurrentRow, 9);
                                cell.Value = option.ImageURL;
                                cell.Style.Font.FontColor = XLColor.Blue;
                                cell.Style.Font.Underline = XLFontUnderlineValues.Single;
                            }
                            optionsWorksheet.Cell(optionCurrentRow, 10).Value = option.Status;

                            optionCurrentRow++;
                        }
                    }

                    variantCurrentRow++;
                }

                variantsWorksheet.Columns().AdjustToContents();
                optionsWorksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Variants_and_Options.xlsx");
                }
            }
        }

        [HttpGet]
        [Route("update_variant")]
        public async Task<IActionResult> Edit(Guid ID)
        {
            var category = await _ICategoryRespository.GetAllActiveAsync();
            ViewBag.Category = category;
            ViewBag.IDVariant = ID;
            var brand = await _IBrandRepository.GetAllActiveAsync();
            ViewBag.Brand = brand;

            var material = await _IMaterialRepository.GetAllActiveAsync();
            ViewBag.Material = material;
            var data = await _IVariantRepository.GetByIDAsync(ID);
            ViewBag.Variant = data;
            return View("~/Areas/Admin/Views/ManagerVariant/Edit.cshtml");
        }
        [HttpPut]
        [Route("update_variant")]
        public async Task<IActionResult> Edit()
        {
            return View();
        }
        [HttpGet]
        [Route("variantlist")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var productdata = await _IVariantRepository.GetAllAsync();
                if (productdata != null)
                {
                    return View("~/Areas/Admin/Views/ManagerVariant/Index.cshtml", productdata);
                }
                else
                {
                    return RedirectToAction("NoData");
                }
            }
            catch (Exception ex)
            {
                // Trả về một trang lỗi
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("createvariant")]
        public async Task<IActionResult> Create()
        {
            string token = HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);
            ViewBag.IDUser = userID;
            var category = await _ICategoryRespository.GetAllActiveAsync();
            ViewBag.Category = category;
            var brand = await _IBrandRepository.GetAllActiveAsync();
            ViewBag.Brand = brand;
            var material = await _IMaterialRepository.GetAllActiveAsync();
            ViewBag.Material = material;
            return View("~/Areas/Admin/Views/ManagerVariant/Create.cshtml");
        }
        [HttpPost]
        [Route("createvariant")]
        public async Task<IActionResult> Create(VariantsCreateVM request)
        {
            return View();
        }
        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> Details(Guid IDVariant)
        {
            var options = await _IVariantRepository.GetOptionVariantByIDAsync(IDVariant);
            var variant = await _IVariantRepository.GetByIDAsync(IDVariant);
            var review = await _IReviewRepository.GetByVariant(IDVariant);
            var view = new ModelCompositeShare()
            {
                VariantsVM = variant,
                LstReviewVM = review,
                LstOptionsVM = options
            };
            return View("~/Areas/Admin/Views/ManagerVariant/Details.cshtml", view);
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
