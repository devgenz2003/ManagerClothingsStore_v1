using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Colors;
using CHERRY.UI.Repositorys._1_Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class ColorsRepository : IColorsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ColorsRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<bool> CreateAsync(ColorsCreateVM request)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            string userID = ExtractUserIDFromToken(token);

            request.CreateBy = userID;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.PostAsJsonAsync("api/Colors/create", request);

            return result.IsSuccessStatusCode;
        }

        public async Task<List<ColorsVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ColorsVM>>("api/Colors/getallactive");
        }

        public async Task<List<ColorsVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ColorsVM>>("api/Colors/getall");
        }

        public async Task<ColorsVM> GetByIDAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<ColorsVM>($"api/Colors/GetByID/{ID}");
        }

        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            var resutl = await _httpClient.DeleteAsync($"api/Colors/{ID}/{IDUserdelete}");
            return resutl.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid ID, ColorsUpdateVM request)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            string userID = ExtractUserIDFromToken(token);

            request.ModifiedBy = userID;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.PutAsJsonAsync($"api/Brand/Colors/{ID}", request);

            return result.IsSuccessStatusCode;
        }
    }
}
