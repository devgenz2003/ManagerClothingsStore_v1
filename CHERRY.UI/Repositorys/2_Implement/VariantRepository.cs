using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Variants;
using CHERRY.UI.Repositorys._1_Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class VariantRepository : IVariantRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VariantRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
                return null; // Trả về null nếu không tìm thấy ID
            }
            catch (Exception ex)
            {
                // Có thể ghi log lỗi ở đây
                return null; // Trả về null trong trường hợp lỗi
            }
        }
      
        public async Task<bool> CreateAsync(VariantsCreateVM request)
        {
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(request.ID.ToString()), "ID");
                formData.Add(new StringContent(request.IDCategory.ToString() ?? ""), "IDCategory");
                formData.Add(new StringContent(request.IDBrand?.ToString() ?? ""), "IDBrand");
                formData.Add(new StringContent(request.IDMaterial?.ToString() ?? ""), "IDMaterial");
                formData.Add(new StringContent(request.VariantName ?? ""), "VariantName");
                formData.Add(new StringContent(request.Description ?? ""), "Description");
                formData.Add(new StringContent(request.MaterialName ?? ""), "MaterialName");
                formData.Add(new StringContent(request.BrandName ?? ""), "BrandName");
                formData.Add(new StringContent(request.Style ?? ""), "Style");
                formData.Add(new StringContent(request.Origin ?? ""), "Origin");
                formData.Add(new StringContent(request.SKU_v2 ?? ""), "SKU_v2");
                formData.Add(new StringContent(request.Status.ToString()), "Status");

                foreach (var file in request.ImagePaths)
                {
                    formData.Add(new StreamContent(file.OpenReadStream()), "ImagePaths", file.FileName);
                }
                string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
                string userID = ExtractUserIDFromToken(token);
                formData.Add(new StringContent(userID ?? ""), "CreateBy");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Variants/create")
                {
                    Content = formData
                };
                var response = await _httpClient.SendAsync(requestMessage);

                return response.IsSuccessStatusCode;
            }
        }

        public async Task<List<VariantsVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<VariantsVM>>("api/Variants/allactive");
        }

        public async Task<List<VariantsVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<VariantsVM>>("api/Variants/all");
        }

        public async Task<VariantsVM> GetByIDAsync(Guid IDVariant)
        {
            return await _httpClient.GetFromJsonAsync<VariantsVM>($"api/Variants/GetByID/{IDVariant}");
        }

        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Guid ID, VariantsUpdateVM request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OptionsVM>> GetOptionVariantByIDAsync(Guid IDVariant)
        {
            return await _httpClient.GetFromJsonAsync<List<OptionsVM>>($"api/Variants/GetVariantByID/{IDVariant}");
        }
    }
}
