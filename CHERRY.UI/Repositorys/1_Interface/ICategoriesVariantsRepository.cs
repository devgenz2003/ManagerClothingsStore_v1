using CHERRY.BUS.ViewModels.CategoriesVariants;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface ICategoriesVariantsRepository
    {
        public Task<List<CategoriesVariantsVM>> GetAllAsync();
        public Task<List<CategoriesVariantsVM>> GetAllActiveAsync();
        public Task<CategoriesVariantsVM> GetByIDAsync(Guid IDVariant, Guid IDCategory);
        public Task<bool> CreateAsync(CategoriesVariantsCreateVM request);
        public Task<bool> RemoveAsync(Guid IDVariant, Guid IDCategory, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, CategoriesVariantsUpdateVM request);
    }
}
