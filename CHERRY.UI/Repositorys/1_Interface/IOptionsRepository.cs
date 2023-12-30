using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Variants;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface IOptionsRepository
    {
        public Task<List<OptionsVM>> GetAllAsync();
        public Task<List<OptionsVM>> GetAllActiveAsync();
        public Task<OptionsVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(OptionsCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, OptionsUpdateVM request); 
        public Task<Guid> GetVariantByID(Guid IDOptions);

    }
}
