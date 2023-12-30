using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Variants;
using CHERRY.UI.Repositorys._1_Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class OptionsRepository : IOptionsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OptionsRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<bool> CreateAsync(OptionsCreateVM request)
        {
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(request.IDVariant.ToString()), "IDVariant");
                formData.Add(new StringContent(request.IDColor?.ToString() ?? ""), "IDColor");
                formData.Add(new StringContent(request.IDSizes?.ToString() ?? ""), "IDSizes");
                formData.Add(new StringContent(request.ColorName ?? ""), "ColorName");
                formData.Add(new StringContent(request.SizesName ?? ""), "SizesName");
                formData.Add(new StringContent(request.Status.ToString()), "Status");
                formData.Add(new StringContent(request.CostPrice.ToString()), "CostPrice");
                formData.Add(new StringContent(request.RetailPrice.ToString()), "RetailPrice");
                formData.Add(new StringContent(request.DiscountedPrice?.ToString() ?? ""), "DiscountedPrice");
                formData.Add(new StringContent(request.StockQuantity.ToString()), "StockQuantity");

                // Thêm file ảnh, nếu có
                if (request.ImagePaths != null)
                {
                    formData.Add(new StreamContent(request.ImagePaths.OpenReadStream()), "ImagePaths", request.ImagePaths.FileName);
                }
                string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
                string userID = ExtractUserIDFromToken(token);
                formData.Add(new StringContent(userID ?? ""), "CreateBy");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Options/create")
                {
                    Content = formData
                };
                var response = await _httpClient.SendAsync(requestMessage);

                return response.IsSuccessStatusCode;
            }
        }


        public async Task<List<OptionsVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<OptionsVM>>("api/Options/getallactive");
        }

        public async Task<List<OptionsVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<OptionsVM>>("api/Options/getall");
        }

        public async Task<OptionsVM> GetByIDAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<OptionsVM>($"api/Options/GetByID/{ID}");
        }

        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Guid ID, OptionsUpdateVM request)
        {
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(request.IDVariant.ToString()), "IDVariant");
                formData.Add(new StringContent(request.IDColor?.ToString() ?? ""), "IDColor");
                formData.Add(new StringContent(request.IDSizes?.ToString() ?? ""), "IDSizes");
                formData.Add(new StringContent(request.ColorName ?? ""), "ColorName");
                formData.Add(new StringContent(request.SizesName ?? ""), "SizesName");
                formData.Add(new StringContent(request.Status.ToString()), "Status");
                formData.Add(new StringContent(request.CostPrice.ToString()), "CostPrice");
                formData.Add(new StringContent(request.RetailPrice.ToString()), "RetailPrice");
                formData.Add(new StringContent(request.DiscountedPrice?.ToString() ?? ""), "DiscountedPrice");
                formData.Add(new StringContent(request.StockQuantity.ToString()), "StockQuantity");

                // Thêm file ảnh, nếu có
                if (request.ImageURL != null)
                {
                    formData.Add(new StreamContent(request.ImageURL.OpenReadStream()), "ImagePaths", request.ImageURL.FileName);
                }
                string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
                formData.Add(new StringContent(ExtractUserIDFromToken(token) ?? ""), "ModifieBy");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsync($"api/Options/update/{ID}", formData);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<Guid> GetVariantByID(Guid IDOptions)
        {
            var response = await _httpClient.GetFromJsonAsync<Guid>($"api/Options/GetVariantByID/{IDOptions}");

            if (response != null)
            {
                return response;
            }
            return Guid.Empty;
        }
    }
}
