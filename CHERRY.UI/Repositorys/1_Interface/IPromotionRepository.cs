using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.BUS.ViewModels.PromotionVariants;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface IPromotionRepository
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
        public Task<decimal> ApplyPromotionAsync(Guid ID, decimal originalPrice);  // Áp dụng khuyến mại
        public Task<List<PromotionVariantsVM>> GetVariantsInPromotionAsync(Guid ID);

    }
}
