using CHERRY.BUS.ViewModels.Material;
using CHERRY.UI.Repositorys._1_Interface;
using System.Net.Http;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly HttpClient _httpClient;

        public MaterialRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreateAsync(MaterialCreateVM request)
        {
            var resutl = await _httpClient.PostAsJsonAsync("api/Material/create", request);
            return resutl.IsSuccessStatusCode;
        }

        public async Task<List<MaterialVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<MaterialVM>>("api/Material/getallactive");
        }

        public async Task<List<MaterialVM>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<MaterialVM>>("api/Material/getall");
        }

        public async Task<MaterialVM> GetByIDAsync(Guid ID)
        {
            return await _httpClient.GetFromJsonAsync<MaterialVM>($"api/Material/GetByID/{ID}");
        }

        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Guid ID, MaterialUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
