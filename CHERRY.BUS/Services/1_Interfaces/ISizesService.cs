using CHERRY.BUS.ViewModels.Sizes;
namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface ISizesService
    {
        public Task<List<SizesVM>> GetAllAsync();
        public Task<List<SizesVM>> GetAllActiveAsync();
        public Task<SizesVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(SizesCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, SizesUpdateVM request);
    }
}
