using CHERRY.BUS.ViewModels.Cart;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface ICartRepository
    {
        public Task<List<CartVM>> GetAllAsync();
        public Task<List<CartVM>> GetAllActiveAsync();
        public Task<CartVM> GetByIDAsync(Guid ID);
        public Task<CartVM> GetCartByUserIDAsync(string IDUser);
        public Task<bool> CreateAsync(CartCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, CartUpdateVM request);
    }
}
