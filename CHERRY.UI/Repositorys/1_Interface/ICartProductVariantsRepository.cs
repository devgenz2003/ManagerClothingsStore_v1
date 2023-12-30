using CHERRY.BUS.ViewModels.CartProductVariants;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface ICartProductVariantsRepository
    {
        public Task<List<CartProductVariantsVM>> GetAllAsync();
        public Task<List<CartProductVariantsVM>> GetAllActiveAsync();
        public Task<CartProductVariantsVM> GetByIDAsync(Guid ID_Cart, Guid ID_ClassifyItem);
        public Task<bool> CreateAsync(CartProductVariantsCreateVM request);
        public Task<bool> RemoveAsync(Guid ID_Cart, Guid ID_ClassifyItem, Guid idUserdelete);
        public Task<bool> UpdateAsync(Guid ID_Cart, Guid ID_ClassifyItem, CartProductVariantsUpdateVM request);
        public Task<List<CartProductVariantsVM>> GetAllByCartIdAsync(Guid ID_Cart);

    }
}
