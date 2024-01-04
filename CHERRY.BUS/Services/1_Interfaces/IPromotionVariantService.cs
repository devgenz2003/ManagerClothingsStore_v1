using CHERRY.BUS.ViewModels.PromotionVariants;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IPromotionVariantService
    {
        public Task<List<PromotionVariantsVM>> GetAllAsync();
        public Task<List<PromotionVariantsVM>> GetAllActiveAsync();
        public Task<PromotionVariantsVM> GetByIDAsync(Guid IDVariant, Guid IDPromotion);
        public Task<bool> CreateAsync(PromotionVariantsCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, PromotionVariantsUpdateVM request);
    }
}
