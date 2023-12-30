using CHERRY.BUS.ViewModels.CartProductVariants;
using CHERRY.BUS.ViewModels.CategoriesVariants;
using CHERRY.BUS.ViewModels.Category;
using CHERRY.UI.Repositorys._1_Interface;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class CategoriesVariantsRepository : ICategoriesVariantsRepository
    {
        private readonly HttpClient _httpClient;

        public CategoriesVariantsRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreateAsync(CategoriesVariantsCreateVM request)
        {
            var resutl = await _httpClient.PostAsJsonAsync("api/CategoriesVariants/create", request);
            return resutl.IsSuccessStatusCode;
        }

        public async Task<List<CategoriesVariantsVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoriesVariantsVM>>("api/CategoriesVariants/getallactive");
        }

        public async Task<List<CategoriesVariantsVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoriesVariantsVM>>("api/CategoriesVariants/getall");
        }

        public async Task<CategoriesVariantsVM> GetByIDAsync(Guid IDVariant, Guid IDCategory)
        {
            return await _httpClient.GetFromJsonAsync<CategoriesVariantsVM>($"api/CategoriesVariants/{IDVariant}/{IDCategory}");
        }

        public async Task<bool> RemoveAsync(Guid IDVariant, Guid IDCategory, string IDUserdelete)
        {
            var resutl = await _httpClient.DeleteAsync($"api/CategoriesVariants/{IDVariant}/{IDCategory}/{IDUserdelete}");
            return resutl.IsSuccessStatusCode;
        }

        public Task<bool> UpdateAsync(Guid ID, CategoriesVariantsUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
