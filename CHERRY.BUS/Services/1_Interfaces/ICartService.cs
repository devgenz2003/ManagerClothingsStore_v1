using CHERRY.BUS.ViewModels.Cart;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface ICartService
    {
        public Task<List<CartVM>> GetAllAsync();
        public Task<List<CartVM>> GetAllActiveAsync();
        public Task<CartVM> GetByIDAsync(Guid ID);
        public Task<CartVM> GetCartByUserIDAsync(string ID_USER);
        public Task<bool> CreateAsync(CartCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, CartUpdateVM request);
    }
}
