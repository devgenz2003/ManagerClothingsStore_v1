using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Material;
using CHERRY.UI.Repositorys._1_Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class BrandRepository : IBrandRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BrandRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<bool> CreateAsync(BrandCreateVM request)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            string userID = ExtractUserIDFromToken(token);

            request.CreateBy = userID;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.PostAsJsonAsync("api/Brand/create", request);

            return result.IsSuccessStatusCode;
        }

        public async Task<List<BrandVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<BrandVM>>("api/Brand/getallactive");
        }

        public async Task<List<BrandVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<BrandVM>>("api/Brand/getall");
        }

        public async Task<BrandVM> GetByIDAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<BrandVM>($"api/Brand/GetByID/{ID}");
        }

        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            var resutl = await _httpClient.DeleteAsync($"api/Brand/{ID}/{IDUserdelete}");
            return resutl.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid ID, BrandUpdateVM request)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            string userID = ExtractUserIDFromToken(token);

            request.ModifiedBy = userID;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.PutAsJsonAsync($"api/Brand/update/{ID}", request);

            return result.IsSuccessStatusCode;
        }
    }
}
