using CHERRY.BUS.ViewModels.OrderVariant;
using CHERRY.UI.Repositorys._1_Interface;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class OrderVariantRepository : IOrderVariantRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderVariantRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> CreateAsync(OrderVariantCreateVM request)
        {
            var resutl = await _httpClient.PostAsJsonAsync("api/OrderVariant/create", request);
            return resutl.IsSuccessStatusCode;
        }

        public Task<List<OrderVariantVM>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderVariantVM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderVariantVM> GetByIDAsync(Guid ID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Guid ID, OrderVariantlUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
