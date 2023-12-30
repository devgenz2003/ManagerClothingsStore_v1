using CHERRY.BUS.ViewModels.Category;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface ICategoryRespository
    {
        public Task<List<CategoryVM>> GetAllAsync();
        public Task<List<CategoryVM>> GetAllActiveAsync();
        public Task<CategoryVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(CategoryCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, CategoryUpdateVM request);
    }
}
