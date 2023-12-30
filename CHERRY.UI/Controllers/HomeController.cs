using CHERRY.BUS.ViewModels.User;
using CHERRY.UI.Areas.Admin.Models;
using CHERRY.UI.Models;
using CHERRY.UI.Repositorys._1_Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CHERRY.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVariantRepository _IVariantRepository;
        private readonly IOptionsRepository _IOptionsRepository;
        private readonly IReviewRepository _IReviewRepository;
        public HomeController(ILogger<HomeController> logger, IVariantRepository IVariantRepository, IOptionsRepository IOptionsRepository, IReviewRepository iReviewRepository)
        {
            _IOptionsRepository = IOptionsRepository;
            _IVariantRepository = IVariantRepository;
            _IReviewRepository = iReviewRepository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productList = await _IVariantRepository.GetAllActiveAsync();
            var model = new ModelCompositeShare()
            {
                LstVariantsVM = productList
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
            #region Regíter
            //try
            //{
            //    var registerUserJSON = JsonConvert.SerializeObject(registerUser);

            //    var stringContent = new StringContent(registerUserJSON, Encoding.UTF8, "application/json");

            //    role = "Client";
            //    var queryString = $"?role={role}";

            //    var response = await _httpClient.PostAsync($"https://localhost:7108/api/Register{queryString}", stringContent);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        ViewBag.Message = "Đăng ký thành công!";
            //        return RedirectToAction("Login", "Home");
            //    }
            //    else
            //    {
            //        ViewBag.Message = "Đã xảy ra lỗi khi đăng ký.";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Xử lý ngoại lệ ở đây, ví dụ:
            //    ViewBag.Message = $"Đã xảy ra lỗi: {ex.Message}";
            //}

            #endregion
            return View(); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
