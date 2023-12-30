using CHERRY.BUS.ViewModels.Brand;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IBrandServices
    {
        public Task<List<BrandVM>> GetAllAsync();
        public Task<List<BrandVM>> GetAllActiveAsync();
        public Task<BrandVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(BrandCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, BrandUpdateVM request);
    }
}
