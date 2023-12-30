using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.BUS.ViewModels.PromotionVariants;
using CHERRY.UI.Repositorys._1_Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PromotionRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<bool> ActivatePromotionAsync(Guid ID)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.PostAsync($"api/Promotion/activate/{ID}", null);
            return result.IsSuccessStatusCode;
        }
        public async Task<decimal> ApplyPromotionAsync(Guid ID, decimal originalPrice)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync($"api/Promotion/apply/{ID}", originalPrice);
            if (response.IsSuccessStatusCode)
            {
                var discountedPrice = await response.Content.ReadFromJsonAsync<decimal>();
                return discountedPrice;
            }
            return originalPrice;
        }
        public async Task<bool> CreateAsync(PromotionCreateVM request)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            string userID = ExtractUserIDFromToken(token);

            request.CreateBy = userID;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.PostAsJsonAsync("api/Promotion/create", request);

            return result.IsSuccessStatusCode;
        }
        public async Task<bool> DeactivatePromotionAsync(Guid ID)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.PostAsync($"api/Promotion/deactivate/{ID}", null);
            return result.IsSuccessStatusCode;
        }
        public async Task<List<PromotionVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PromotionVM>>("api/Promotion/getallactive");
        }
        public async Task<List<PromotionVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PromotionVM>>("api/Promotion/getall");
        }
        public async Task<PromotionVM> GetByIDAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<PromotionVM>($"api/Promotion/GetByID/{ID}");
        }
        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            var resutl = await _httpClient.DeleteAsync($"api/Promotion/{ID}/{IDUserdelete}");
            return resutl.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateAsync(Guid ID, PromotionUpdateVM request)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            string userID = ExtractUserIDFromToken(token);

            request.ModifiedBy = userID;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.PutAsJsonAsync($"api/Promotion/update/{ID}", request);

            return result.IsSuccessStatusCode;
        }
        public async Task<bool> ValidatePromotionAsync(Guid ID)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/Promotion/validate/{ID}");
            return response.IsSuccessStatusCode;
        }
        public async Task<List<PromotionVariantsVM>> GetVariantsInPromotionAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<List<PromotionVariantsVM>>($"api/Promotion/{ID}/Variants");
        }
    }
}
