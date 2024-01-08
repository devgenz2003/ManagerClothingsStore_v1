using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.BUS.ViewModels.PromotionVariants;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IPromotionService
    {
        public Task<List<PromotionVM>> GetAllAsync();
        public Task<List<PromotionVM>> GetAllActiveAsync();
        public Task<PromotionVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(PromotionCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, PromotionUpdateVM request);
        public Task<bool> ActivatePromotionAsync(Guid ID);  // Kích hoạt khuyến mại
        public Task<bool> DeactivatePromotionAsync(Guid ID);  // Hủy kích hoạt khuyến mại
        public Task<bool> ValidatePromotionAsync(Guid ID);  // Kiểm tra tính hợp lệ của khuyến mại
        public Task<decimal> ApplyPromotionAsync(Guid promotionID, List<Guid> variantIDs, decimal originalPrice);
        public Task<List<PromotionVariantsVM>> GetVariantsInPromotionAsync(Guid ID);
    }
}
