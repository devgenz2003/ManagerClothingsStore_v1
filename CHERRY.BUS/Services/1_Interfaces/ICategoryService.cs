
using CHERRY.BUS.ViewModels.Category;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface ICategoryService
    {
        public Task<List<CategoryVM>> GetAllAsync();
        public Task<List<CategoryVM>> GetAllActiveAsync();
        public Task<CategoryVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(CategoryCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, CategoryUpdateVM request);
    }
}
