using CHERRY.BUS.ViewModels.Colors;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface IColorsRepository
    {
        public Task<List<ColorsVM>> GetAllAsync();
        public Task<List<ColorsVM>> GetAllActiveAsync();
        public Task<ColorsVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(ColorsCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, ColorsUpdateVM request);
    }
}
