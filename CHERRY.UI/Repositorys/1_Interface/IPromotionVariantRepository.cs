using CHERRY.BUS.ViewModels.PromotionVariants;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface IPromotionVariantRepository
    {
        public Task<List<PromotionVariantsVM>> GetAllAsync();
        public Task<List<PromotionVariantsVM>> GetAllActiveAsync();
        public Task<PromotionVariantsVM> GetByIDAsync(Guid IDVariant, Guid IDPromotion);
        public Task<bool> CreateAsync(PromotionVariantsCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, PromotionVariantsUpdateVM request);

    }
}
