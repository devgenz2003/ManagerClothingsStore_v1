using CHERRY.BUS.ViewModels.OrderVariant;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IOrderVariantService
    {
        public Task<List<OrderVariantVM>> GetAllAsync();
        public Task<List<OrderVariantVM>> GetAllActiveAsync();
        public Task<OrderVariantVM> GetByIDAsync(Guid ID);
        public Task<bool> CreateAsync(OrderVariantCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, OrderVariantlUpdateVM request);
    }
}
