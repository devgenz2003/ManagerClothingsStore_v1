using CHERRY.UI.Areas.Admin.Models;
using CHERRY.UI.Models;
using CHERRY.UI.Repositorys._1_Interface;
using CHERRY.UI.Repositorys._2_Implement;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.UI.Areas.Admin.Controllers
{
    [Area("admin")]

    public class ManagerUserController : Controller
    {
        private readonly IUserResponse _IUserRepository;
        private readonly IOrderRepository _IOrderRepository;

        public ManagerUserController(IUserResponse IUserRepository, IOrderRepository IOrderRepository)
        {
            _IOrderRepository = IOrderRepository;
            _IUserRepository = IUserRepository;
        }
        [HttpGet]
        [Route("user_list")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _IUserRepository.GetAllActiveAsync();

                if (data == null || !data.Any())
                {
                    ViewBag.ErrorMessage = "Không có dữ liệu người dùng.";
                    return View("~/Areas/Admin/Views/ManagerUser/Index.cshtml", new ModelCompositeShare());
                }

                var model = new ModelCompositeShare
                {
                    LstUser = data
                };

                return View("~/Areas/Admin/Views/ManagerUser/Index.cshtml", model);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = "Có lỗi xảy ra khi lấy danh sách người dùng." });
            }
        }
        [HttpGet]
        [Route("user_details/{IDUser}")]
        public async Task<IActionResult> Details(string IDUser)
        {
            try
            {
                var user = await _IUserRepository.GetByIDAsync(IDUser);

                if (user == null)
                {
                    return NotFound("Không tìm thấy người dùng với ID này.");
                }
                var orders = await _IOrderRepository.GetByCustomerIDAsync(IDUser);

                var viewModel = new ModelCompositeShare
                {
                    UserVM = user,
                    LstOrderVM = orders
                };

                return View("~/Areas/Admin/Views/ManagerUser/Details.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = "Có lỗi xảy ra khi lấy thông tin chi tiết người dùng." });
            }
        }
    }
}
