using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Review;
using CHERRY.UI.Repositorys._1_Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
                return null;
            }
        }
        public async Task<bool> CreateAsync(ReviewCreateVM request)
        {
            using (var formData = new MultipartFormDataContent())
            {

                formData.Add(new StringContent(request.IDVariant.ToString()), "IDOptions");
                formData.Add(new StringContent(request.IDOrderVariant.ToString()), "IDOrderVariant");
                formData.Add(new StringContent(request.IDUser.ToString() ?? ""), "IDUser");
                formData.Add(new StringContent(request.Content ?? ""), "Content");
                formData.Add(new StringContent(request.Rating.ToString() ?? ""), "Rating");
                formData.Add(new StringContent(request.IsPurchased.ToString() ?? ""), "IsPurchased");
                formData.Add(new StringContent(request.Status.ToString()), "Status");
                foreach (var file in request.ImagePaths)
                {
                    formData.Add(new StreamContent(file.OpenReadStream()), "ImagePaths", file.FileName);
                }
                string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
                string userID = ExtractUserIDFromToken(token);
                formData.Add(new StringContent(userID ?? ""), "CreateBy");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Review/create")
                {
                    Content = formData
                };
                var response = await _httpClient.SendAsync(requestMessage);

                return response.IsSuccessStatusCode;
            }
        }

        public async Task<List<ReviewVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ReviewVM>>("api/Review/getallactive");
        }

        public async Task<List<ReviewVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ReviewVM>>("api/Review/getall");
        }

        public async Task<ReviewVM> GetByIDAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<ReviewVM>($"api/Review/GetByID/{ID}");
        }
        public async Task<List<ReviewVM>> GetByVariant(Guid IDVariant)
        {
            return await _httpClient.GetFromJsonAsync<List<ReviewVM>>($"api/Review/GetByVariant/{IDVariant}");
        }

        public async Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete)
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Gửi yêu cầu DELETE
            var response = await _httpClient.DeleteAsync($"api/Review/remove/{ID}/{IDUserdelete}");

            // Trả về true nếu yêu cầu thành công
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid ID, ReviewUpdateVM request)
        {
            // Thiết lập header Authorization nếu cần
            string token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Gửi yêu cầu PUT
            var response = await _httpClient.PutAsJsonAsync($"api/Review/update/{ID}", request);

            // Trả về true nếu yêu cầu thành công
            return response.IsSuccessStatusCode;
        }

        public async Task<ReviewVM> GetByUser(string IDUser)
        {
            return await _httpClient.GetFromJsonAsync<ReviewVM>($"api/Review/GetByUser/{IDUser}");
        }
    }
}
