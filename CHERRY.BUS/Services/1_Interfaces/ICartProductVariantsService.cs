using CHERRY.BUS.ViewModels.CartProductVariants;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface ICartProductVariantsService
    {
        public Task<List<CartProductVariantsVM>> GetAllAsync();
        public Task<List<CartProductVariantsVM>> GetAllActiveAsync();
        public Task<CartProductVariantsVM> GetByIDAsync(Guid IDCart, Guid? IDOptions);
        public Task<bool> CreateAsync(CartProductVariantsCreateVM request);
        public Task<bool> RemoveAsync(Guid IDCart, Guid? IDOptions ,Guid idUserdelete);
        public Task<bool> UpdateAsync(Guid IDCart, Guid? IDOptions, CartProductVariantsUpdateVM request);
        public Task<List<CartProductVariantsVM>> GetAllByCartIDAsync(Guid IDCart);
    }
}
