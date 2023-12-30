using CHERRY.BUS.ViewModels.Sizes;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface ISizesRepository
    {
        public Task<List<SizesVM>> GetAllAsync();
        public Task<List<SizesVM>> GetAllActiveAsync();
        public Task<SizesVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(SizesCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, SizesUpdateVM request);
    }
}
