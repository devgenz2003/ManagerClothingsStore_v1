
using CHERRY.BUS.ViewModels.Material;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IMaterialServices
    {
        public Task<List<MaterialVM>> GetAllAsync();
        public Task<List<MaterialVM>> GetAllActiveAsync();
        public Task<MaterialVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(MaterialCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, MaterialUpdateVM request);
    }
}
