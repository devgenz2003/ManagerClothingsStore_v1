using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Variants;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IOptionsService
    {
        public Task<List<OptionsVM>> GetAllAsync();
        public Task<List<OptionsVM>> GetAllActiveAsync();
        public Task<OptionsVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(OptionsCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, OptionsUpdateVM request);
        public Task<Guid> GetVariantByID(Guid IDOptions);
        public Task<OptionsVM> FindIDOptionsAsync(Guid IDVariant, string size, string color);

    }
}
