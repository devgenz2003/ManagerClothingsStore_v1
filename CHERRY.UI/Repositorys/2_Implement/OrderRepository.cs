using CHERRY.BUS.ViewModels.Order;
using CHERRY.BUS.ViewModels.OrderVariant;
using CHERRY.DAL.Entities;
using CHERRY.UI.Repositorys._1_Interface;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class OrderRepository : IOrderRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> CreateAsync(OrderCreateVM request)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);

            if (userID != string.Empty)
            {
                request.CreateBy = Guid.NewGuid();
                request.ID = Guid.NewGuid();
                request.IDUser = userID;
                request.TotalAmount = request.OrderVariantCreateVM.Sum(od => od.TotalAmount);
                var orderResponse = await _httpClient.PostAsJsonAsync("api/Order/create", request);
                if (orderResponse.IsSuccessStatusCode)
                {
                    foreach (var orderDetail in request.OrderVariantCreateVM)
                    {
                        orderDetail.IDOrder = request.ID;
                        orderDetail.ID = Guid.NewGuid();
                        orderDetail.Status = 1;
                        var orderDetailResponse = await _httpClient.PostAsJsonAsync("api/OrderVariant/create", orderDetail);

                        if (!orderDetailResponse.IsSuccessStatusCode)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
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
                    if (userIDClaim != null)
                    {
                        return userIDClaim.Value;
                    }
                }
                return null; // Trả về null nếu không tìm thấy ID
            }
            catch (Exception ex)
            {
                // Có thể ghi log lỗi ở đây
                return null; // Trả về null trong trường hợp lỗi
            }
        }
        public async Task<List<OrderVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<OrderVM>>("api/Order/allactive");
        }
        public async Task<List<OrderVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<OrderVM>>("api/Order/all");
        }
        public async Task<List<OrderVM>> GetByCustomerIDAsync(string ID_User)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            string userID = ExtractUserIDFromToken(token);

            return await _httpClient.GetFromJsonAsync<List<OrderVM>>($"api/Order/customer/{userID}");
        }
        public Task<List<OrderVM>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
        public async Task<OrderVM> GetByIDAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<OrderVM>($"api/Order/GetByID/{ID}");
        }
        public Task<List<OrderVM>> GetByStatusAsync(OrderStatus status)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> MarkAsCancelledAsync(Guid ID_Order)
        {
            try
            {
                // Gửi yêu cầu HTTP để đánh dấu đơn hàng với ID_Order là đã bị hủy
                var response = await _httpClient.PutAsync($"api/Order/MarkAsCancelled/{ID_Order}", null);

                // Kiểm tra xem yêu cầu có thành công không
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Xử lý trường hợp xảy ra lỗi trong quá trình gửi yêu cầu
                return false;
            }
        }
        public async Task<bool> MarkAsDeliveredAsync(Guid ID_Order)
        {
            try
            {
                // Gửi yêu cầu HTTP để đánh dấu đơn hàng với ID_Order là đã được giao hàng
                var response = await _httpClient.PutAsync($"api/Order/MarkAsDelivered/{ID_Order}", null);

                // Kiểm tra xem yêu cầu có thành công không
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Xử lý trường hợp xảy ra lỗi trong quá trình gửi yêu cầu
                return false;
            }
        }
        public async Task<bool> MarkAsReturnedAsync(Guid ID_Order)
        {
            try
            {
                // Gửi yêu cầu HTTP để đánh dấu đơn hàng với ID_Order là đã được trả lại
                var response = await _httpClient.PutAsync($"api/Order/MarkAsReturned/{ID_Order}", null);

                // Kiểm tra xem yêu cầu có thành công không
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Xử lý trường hợp xảy ra lỗi trong quá trình gửi yêu cầu
                return false;
            }
        }
        public async Task<bool> MarkAsPaymentSuccessAsync(string HexCode)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/Order/MarkAsPaymentSuccessAsync/{HexCode}", null);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(Guid ID, OrderUpdateVM request)
        {
            throw new NotImplementedException();
        }
        public async Task<List<OrderVariantVM>> GetOrderVariantByIDAsync(Guid IDOrder)
        {
            return await _httpClient.GetFromJsonAsync<List<OrderVariantVM>>($"api/Order/GetOrderDetailsByID/{IDOrder}");
        }
        public async Task<bool> ConfirmOrderAsync(Guid IDOrder, bool trackingCheck)
        {
            try
            {
                var data = new { IDOrder, trackingCheck = true };
                var response = await _httpClient.PatchAsync($"api/Order/{IDOrder}/confirm", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<OrderVM> GetByHexCodeAsync(string HexCode)
        {
            return await _httpClient.GetFromJsonAsync<OrderVM>($"api/Order/GetByHexCode/{HexCode}");
        }
    }
}
