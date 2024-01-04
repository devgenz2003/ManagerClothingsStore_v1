using CHERRY.BUS.ViewModels.CategoriesVariants;
using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.PromotionVariants;
using CHERRY.UI.Repositorys._1_Interface;

namespace CHERRY.UI.Repositorys._2_Implement
{
    public class PromotionVariantRepository : IPromotionVariantRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PromotionVariantRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<bool> CreateAsync(PromotionVariantsCreateVM request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PromotionVariantsVM>> GetAllActiveAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PromotionVariantsVM>>("api/PromotionVariant/getallactive");
        }

        public Task<List<PromotionVariantsVM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PromotionVariantsVM> GetByIDAsync(Guid IDVariant, Guid IDPromotion)
        {
            return await _httpClient.GetFromJsonAsync<PromotionVariantsVM>($"api/PromotionVariant/{IDVariant}/{IDPromotion}");
        }

        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Guid ID, PromotionVariantsUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
