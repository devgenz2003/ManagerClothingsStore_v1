using CHERRY.BUS.ViewModels.User;
using CHERRY.UI.Repositorys._1_Interface;
using System.Net.Http.Json;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class UserResponse : IUserResponse
    {
        private readonly HttpClient _httpClient;

        public UserResponse(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ChangePasswordAsync(string IDUser, ChangePasswordViewModel model)
        {
            try
            {
                var url = $"api/User/{IDUser}/changepassword";

                var response = await _httpClient.PostAsJsonAsync(url, model);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    // Xử lý lỗi hoặc ghi log ở đây nếu cần thiết
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                // Xử lý exception ở đây nếu cần thiết
                return false;
            }
        }

        public async Task<bool> ChangeUserRoleAsync(string IDUser, string newRole)
        {
            try
            {
                var request = new UserRoleChangeRequestModel { NewRole = newRole };
                var url = $"api/User/{IDUser}/changerole";

                var response = await _httpClient.PostAsJsonAsync(url, request);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    // Xử lý lỗi hoặc ghi log ở đây nếu cần thiết
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                // Xử lý exception ở đây nếu cần thiết
                return false;
            }
        }

        public async Task<bool> CreateAsync(UserCreateVM request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/User/create", request);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }


        public async Task<List<UserVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserVM>>("api/User/getallactive");
        }

        public async Task<List<UserVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserVM>>("api/User/getall");
        }


        public async Task<UserVM> GetByGmailAsync(string Gmail)
        {
            return await _httpClient.GetFromJsonAsync<UserVM>($"api/User/GetByGmail/{Gmail}");
        }

        public async Task<UserVM> GetByIDAsync(string ID)
        {
            return await _httpClient.GetFromJsonAsync<UserVM>($"api/User/GetByID/{ID}");
        }

        public async Task<bool> RemoveAsync(string ID, Guid IDUserdelete)
        {

            var resutl = await _httpClient.DeleteAsync($"api/User/{ID}/{IDUserdelete}");
            return resutl.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(string ID, UserUpdateVM request)
        {
            var resutl = await _httpClient.PutAsJsonAsync($"api/User/{ID}", request);
            return resutl.IsSuccessStatusCode;
        }
    }
}
