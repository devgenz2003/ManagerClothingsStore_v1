using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Voucher;
using CHERRY.UI.Repositorys._1_Interface;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VoucherRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

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
                return "Không có dữ liệu"; 
            }
            catch (Exception ex)
            {
                return "Không có dữ liệu";
            }
        }
        public async Task<bool> ActivateVoucherAsync(Guid ID)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> CreateAsync(VoucherCreateVM request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            if (string.IsNullOrEmpty(token))
            {
                return false; 
            }
            string userID = ExtractUserIDFromToken(token);
            request.CreateBy = userID; 
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/Voucher/create", request);
                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Task<bool> DeactivateVoucherAsync(Guid ID)
        {
            throw new NotImplementedException();
        }
        public async Task<List<VoucherVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<VoucherVM>>("api/Voucher/getallactive");
        }
        public async Task<List<VoucherVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<VoucherVM>>("api/Voucher/getall");
        }
        public async Task<VoucherVM> GetByIDAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<VoucherVM>($"api/Voucher/GetByID/{ID}");
        }
        public Task<List<VoucherVM>> GetVouchersByExpirationDateAsync(DateTime expirationDate)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete)
        {
            var resutl = await _httpClient.DeleteAsync($"api/Voucher/{ID}/{IDUserdelete}");
            return resutl.IsSuccessStatusCode;
        }
        public Task<List<VoucherVM>> SearchVouchersAsync(string keyword)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateAsync(Guid ID, VoucherUpdateVM request)
        {
            var resutl = await _httpClient.PutAsJsonAsync($"api/Voucher/{ID}", request);
            return resutl.IsSuccessStatusCode;
        }
        public async Task<List<VoucherVM>> GetVoucherByUser(string IDUser)
        {
            return await _httpClient.GetFromJsonAsync<List<VoucherVM>>($"api/Voucher/GetVoucherByIDUser/{IDUser}");
        }
    }
}
