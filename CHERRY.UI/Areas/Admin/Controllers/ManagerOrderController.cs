using CHERRY.DAL.Entities;
using CHERRY.UI.Areas.Admin.Models;
using CHERRY.UI.Models;
using CHERRY.UI.Repositorys._1_Interface;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.UI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ManagerOrderController : Controller
    {
        private readonly IOrderRepository _IOrderRepository;
        private readonly IVariantRepository _IVariantRepository;
        public ManagerOrderController(IOrderRepository IOrderRepository,
            IVariantRepository IVariantRepository)
        {
            _IOrderRepository = IOrderRepository;
            _IVariantRepository = IVariantRepository;
        }
        [HttpGet]
        [Route("orderlist")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _IOrderRepository.GetAllActiveAsync();

                if (data == null || !data.Any())
                {
                    ViewBag.ErrorMessage = "Không có dữ liệu";
                }

                return View("~/Areas/Admin/Views/ManagerOrder/Index.cshtml", data);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = "Có lỗi xảy ra khi lấy danh sách" });
            }
        }
        [HttpGet]
        [Route("order_details/{IDOrder}")]
        public async Task<IActionResult> Details_Order(Guid IDOrder)
        {
            try
            {
                var order = await _IOrderRepository.GetByIDAsync(IDOrder); // Giả định bạn có method này
                var orderDetails = await _IOrderRepository.GetOrderVariantByIDAsync(IDOrder);

                if (order == null || orderDetails == null)
                {
                    ViewBag.ErrorMessage = "Không có dữ liệu chi tiết đơn hàng.";
                    return View("Error");
                }

                var viewModel = new ModelCompositeShare
                {
                    OrderVM = order,
                    LstOrderVariantVM = orderDetails
                };

                return View("~/Areas/Admin/Views/ManagerOrder/Details_Order.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Lỗi: {ex.Message}";
                return View("Error");
            }
        }
        [HttpPost]
        [Route("MarkAsCancelled")]
        public async Task<IActionResult> MarkAsCancelled(Guid ID_Order)
        {
            try
            {
                var order = await _IOrderRepository.GetByIDAsync(ID_Order); // Giả sử có phương thức này

                if (order == null)
                {
                    ViewBag.ErrorMessage = "Đơn hàng không tồn tại.";
                    return View("Error");
                }

                if (order.OrderStatus == OrderStatus.Cancelled || order.OrderStatus == OrderStatus.Delivered)
                {
                    return RedirectToAction("Index");
                }


                var success = await _IOrderRepository.MarkAsCancelledAsync(ID_Order);

                if (!success)
                {
                    ViewBag.ErrorMessage = "Không thể đánh dấu là đã hủy đơn hàng.";
                    return View("Error");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Lỗi: {ex.Message}";
                return View("Error");
            }
        }
        [HttpPost]
        [Route("MarkAsReturned")]
        public async Task<IActionResult> MarkAsReturned(Guid ID_Order)
        {
            try
            {
                var order = await _IOrderRepository.GetByIDAsync(ID_Order); // Giả định bạn có phương thức này

                if (order == null)
                {
                    TempData["ErrorMessage"] = "Đơn hàng không tồn tại.";
                    return RedirectToAction("Index");
                }

                if (order.OrderStatus == OrderStatus.Cancelled)
                {
                    TempData["ErrorMessage"] = "Không thể trả lại một đơn hàng đã hủy.";
                    return RedirectToAction("Index");
                }

                var success = await _IOrderRepository.MarkAsReturnedAsync(ID_Order);

                if (!success)
                {
                    TempData["ErrorMessage"] = "Không thể đánh dấu là đã trả lại.";
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [Route("MarkAsShipped")]
        public async Task<IActionResult> MarkAsShipped(Guid ID_Order)
        {
            try
            {
                var order = await _IOrderRepository.GetByIDAsync(ID_Order);

                if (order == null)
                {
                    TempData["ErrorMessage"] = "";
                    return RedirectToAction("Index");
                }

                // Kiểm tra nếu đơn hàng đã hoàn thành
                if (order.OrderStatus == OrderStatus.Cancelled || order.OrderStatus == OrderStatus.Returned || order.OrderStatus == OrderStatus.Delivered)
                {
                    TempData["ErrorMessage"] = "";
                    return RedirectToAction("Index");
                }

                var success = await _IOrderRepository.MarkAsShippedAsync(ID_Order);

                if (!success)
                {
                    TempData["ErrorMessage"] = "";
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [Route("MarkAsDelivered")]
        public async Task<IActionResult> MarkAsDelivered(Guid ID_Order)
        {
            try
            {
                var order = await _IOrderRepository.GetByIDAsync(ID_Order);

                if (order == null)
                {
                    TempData["ErrorMessage"] = "";
                    return RedirectToAction("Index");
                }

                // Kiểm tra nếu đơn hàng đã hoàn thành
                if (order.OrderStatus == OrderStatus.Cancelled || order.OrderStatus == OrderStatus.Returned || order.OrderStatus == OrderStatus.Delivered)
                {
                    TempData["ErrorMessage"] = "";
                    return RedirectToAction("Index");
                }

                var success = await _IOrderRepository.MarkAsDeliveredAsync(ID_Order);

                if (!success)
                {
                    TempData["ErrorMessage"] = "";
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
