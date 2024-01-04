using CHERRY.BUS.ViewModels.CategoriesVariants;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface ICategoriesVariantsService
    {
        public Task<List<CategoriesVariantsVM>> GetAllAsync();
        public Task<List<CategoriesVariantsVM>> GetAllActiveAsync();
        public Task<CategoriesVariantsVM> GetByIDAsync(Guid IDVariant, Guid IDCategory);
        public Task<bool> CreateAsync(CategoriesVariantsCreateVM request);
        public Task<bool> RemoveAsync(Guid IDVariant, Guid IDCategory, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, CategoriesVariantsUpdateVM request);
        public Task<List<CategoriesVariantsVM>> GetMinMaxRetails(decimal MinPrice, decimal MaxPrice);
        public Task<Tuple<decimal, decimal>> GetMinMaxPricesForCategory(Guid IDCategory);

    }
}
