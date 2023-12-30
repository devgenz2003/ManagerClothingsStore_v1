using CHERRY.BUS.ViewModels.Colors;
using CHERRY.BUS.ViewModels.Sizes;
using CHERRY.UI.Repositorys._1_Interface;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class SizesRepository : ISizesRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SizesRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<bool> CreateAsync(SizesCreateVM request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SizesVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<SizesVM>>("api/Sizes/getallactive");
        }

        public Task<List<SizesVM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SizesVM> GetByIDAsync(Guid ID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Guid ID, SizesUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
