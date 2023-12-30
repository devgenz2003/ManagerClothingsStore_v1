using CHERRY.BUS.Services._1_Interface;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.Services.SignalR;
using CHERRY.BUS.ViewModels.Order;
using CHERRY.DAL.Entities;
using CHERRY.UI.Areas.Admin.Models;
using CHERRY.UI.Models;
using CHERRY.UI.Repositorys._1_Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PagedList;
namespace CHERRY.UI.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _IOrderRepository;
        private readonly IOrderVariantRepository _IOrderDetailsRepository;
        private readonly IOptionsRepository _IOptionsRepository;
        private readonly IVariantRepository _IVariantRepository;
        private readonly IUserResponse _IUserResponse;
        private readonly ICartRepository _ICartRepository;
        private readonly ICartProductVariantsRepository _ICartProductVariantsRepository;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IVnPayService _vnPayService;
        private readonly IVoucherRepository _IVoucherRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderController(IHubContext<ChatHub> hubContext, IHttpContextAccessor IHttpContextAccessor,
            IVoucherRepository IVoucherRepository,
            ILogger<OrderController> logger, IOrderVariantRepository IOrderDetailsRepository,
            IUserResponse IUserResponse, IOrderRepository IOrderRepository,
            IVariantRepository IVariantRepository,
            ICartRepository cartRepository, IOptionsRepository IOptionsRepository,
            ICartProductVariantsRepository cartProductVariantsRepository, IVnPayService IVnPayService)
        {
            _httpContextAccessor = IHttpContextAccessor;
            _vnPayService = IVnPayService;
            _hubContext = hubContext;
            _logger = logger;
            _IVoucherRepository = IVoucherRepository;
            _IOrderDetailsRepository = IOrderDetailsRepository;
            _IVariantRepository = IVariantRepository;
            _IOrderRepository = IOrderRepository;
            _IUserResponse = IUserResponse;
            _ICartRepository = cartRepository;
            _ICartProductVariantsRepository = cartProductVariantsRepository;
            _IOptionsRepository = IOptionsRepository;
        }
        [HttpPost]
        [Route("CreatePaymentUrl")]
        public IActionResult CreatePaymentUrl([FromBody] OrderCreateVM model)
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(new { PaymentUrl = paymentUrl });
        }
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = await _vnPayService.PaymentExecute(Request.Query);

            if (response != null)
            {
                return View("Success");
            }

            return BadRequest("Có lỗi nhỏ xảy ra!");
        }
        public async Task CheckAndCancelOrder(Guid orderId)
        {
            var order = await _IOrderRepository.GetByIDAsync(orderId);
            if (order != null && order.OrderStatus == OrderStatus.Pending)
            {
                var success = await _IOrderRepository.MarkAsCancelledAsync(orderId);
            }
        }
        private void ClearCartCookie()
        {
            Response.Cookies.Delete("CartProduct");
        }
        [HttpGet]
        [Route("PDFOrder/{ID}")]
        public async Task<IActionResult> PrintOrderPdf(Guid ID)
        {
            var order = await _IOrderRepository.GetByIDAsync(ID);
            var orderDetails = await _IOrderRepository.GetOrderVariantByIDAsync(ID);

            var model = new CompositeViewModel_Client
            {
                orderVM = order,
                orderVariantVMs = orderDetails
            };

            return new ViewAsPdf("PrintOrderPdf", model)
            {
                FileName = "Order.pdf"
            };
        }
        [HttpPost]
        [Route("ConfirmOrder")]
        public async Task<IActionResult> ConfirmOrder(Guid IDOrder, bool trackingCheck)
        {
            var result = await _IOrderRepository.ConfirmOrderAsync(IDOrder, trackingCheck);

            if (result)
            {
                return RedirectToAction("Index", "Order");
            }
            else
            {
                return View("ConfirmOrderFailure");
            }
        }
        [HttpGet]
        [Route("order_list")]
        public async Task<IActionResult> Index(int? page)
        {
            string token = HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);

            if (userID == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            var data = await _IOrderRepository.GetByCustomerIDAsync(userID);
            return PartialView(data.ToPagedList(pageNumber, pageSize));
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
        [HttpGet]
        [Route("GetOrderDetailsByID/{IDOrder}")]
        public async Task<IActionResult> OrderDetails(Guid IDOrder)
        {
            try
            {
                var order = await _IOrderRepository.GetByIDAsync(IDOrder);
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

                return View("~/Views/Order/OrderDetails.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Lỗi: {ex.Message}";
                return View("Error");
            }
        }
        [HttpGet]
        [Route("checkout")]
        public async Task<IActionResult> Checkout()
        {
            string token = HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);
            ViewBag.IDUser = userID;
            ViewBag.ID = Guid.NewGuid();
            ViewBag.HexCode = GenerateHexCode();
            ViewBag.ShippingMethod = Enum.GetValues(typeof(ShippingMethod))
                .Cast<ShippingMethod>()
                .Select(dt => new SelectListItem
                {
                    Value = ((int)dt).ToString(),
                    Text = dt.GetDisplayName()
                })
                .ToList();
            int GenerateHexCode()
            {
                var now = DateTime.Now;
                var dateString = now.ToString("yyyyMMdd"); 

                var random = new Random();
                var randomPart = random.Next(1000, 9999);

                var hexString = dateString + randomPart.ToString();

                if (int.TryParse(hexString, out int hexCode))
                {
                    return hexCode;
                }
                else
                {
                    randomPart = random.Next(100, 999);
                    hexString = dateString.Substring(2) + randomPart.ToString();
                    return int.Parse(hexString);
                }
            }
            var voucher = await _IVoucherRepository.GetVoucherByUser(userID);
            ViewBag.VoucherData = voucher;

            return View("~/Views/Order/Checkout.cshtml");
        }
        [HttpPost]
        [Route("checkout")]
        public async Task<IActionResult> Checkout(OrderCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ." });
            }
            try
            {
                return Json(new { success = true, message = "Đơn hàng đã được đặt thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Xảy ra lỗi khi xử lý đơn hàng." });
            }
        }
    }
}
