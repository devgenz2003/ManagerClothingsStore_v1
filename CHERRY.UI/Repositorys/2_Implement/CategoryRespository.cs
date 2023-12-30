using CHERRY.BUS.ViewModels.Category;
using CHERRY.UI.Repositorys._1_Interface;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class CategoryRespository : ICategoryRespository
    {
        private readonly HttpClient _httpClient;

        public CategoryRespository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateAsync(CategoryCreateVM request)
        {
            var resutl = await _httpClient.PostAsJsonAsync("api/Category/create", request);
            return resutl.IsSuccessStatusCode;
        }

        public async Task<List<CategoryVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryVM>>("api/Category/getallactive");
        }

        public async Task<List<CategoryVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryVM>>("api/Category/all");
        }

        public async Task<CategoryVM> GetByIDAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<CategoryVM>($"api/Category/getbyid/{ID}");
        }

        public async Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete)
        {
            var resutl = await _httpClient.DeleteAsync($"api/Category/{ID}/{IDUserdelete}");
            return resutl.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid ID, CategoryUpdateVM request)
        {
            var resutl = await _httpClient.PutAsJsonAsync($"api/Category/{ID}", request);
            return resutl.IsSuccessStatusCode;
        }
    }
}
