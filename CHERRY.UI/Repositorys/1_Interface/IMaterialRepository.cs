using CHERRY.BUS.ViewModels.Material;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface IMaterialRepository
    {
        public Task<List<MaterialVM>> GetAllAsync();
        public Task<List<MaterialVM>> GetAllActiveAsync();
        public Task<MaterialVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(MaterialCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, MaterialUpdateVM request);
    }
}
