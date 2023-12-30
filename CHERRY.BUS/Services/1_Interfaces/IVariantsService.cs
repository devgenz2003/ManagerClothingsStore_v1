using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Variants;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IVariantsService
    {
        public Task<List<VariantsVM>> GetAllAsync();
        public Task<List<VariantsVM>> GetAllActiveAsync();
        public Task<VariantsVM> GetByIDAsync(Guid ID);
        public Task<List<OptionsVM>> GetOptionVariantByIDAsync(Guid IDVariant);
        public Task<bool> CreateAsync(VariantsCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, VariantsUpdateVM request);
    }
}
