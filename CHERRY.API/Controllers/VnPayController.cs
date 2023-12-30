//using CHERRY.BUS.Services._1_Interfaces;
//using CHERRY.BUS.ViewModels.Order;
//using CHERRY.BUS.ViewModels.Payment;
//using Microsoft.AspNetCore.Mvc;

//namespace CHERRY.UI.Controllers
//{
//    [Route("api/vnpay")]
//    [ApiController]
//    public class VnPayController : ControllerBase
//    {
//        private readonly IVnPayService _vnPayService;

//        public VnPayController(IVnPayService vnPayService)
//        {
//            _vnPayService = vnPayService;
//        }

//        [HttpPost("create-payment-url")]
//        public IActionResult CreatePaymentUrl([FromBody] OrderCreateVM model)
//        {
//            if (model == null)
//            {
//                return BadRequest("Invalid request body");
//            }

//            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);

//            if (string.IsNullOrEmpty(paymentUrl))
//            {
//                return BadRequest("Failed to generate payment URL"); // Xử lý khi không có URL được tạo thành công
//            }

//            var responseDTO = new PaymentUrlResponseDTO { PaymentUrl = paymentUrl };
//            return Ok(responseDTO); // Trả về DTO chứa PaymentUrl
//        }

//        [HttpPost("execute-payment")] 
//        public async Task<IActionResult> ExecutePayment([FromQuery] IQueryCollection collections)
//        {
//            if (collections == null)
//            {
//                return BadRequest("Invalid query parameters");
//            }

//            var response = await _vnPayService.PaymentExecute(collections);

//            return Ok(response);
//        }
//    }
//}
