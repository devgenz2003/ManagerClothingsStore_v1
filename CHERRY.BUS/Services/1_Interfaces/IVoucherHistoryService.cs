using CHERRY.BUS.ViewModels.VoucherHistory;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IVoucherHistoryService
    {
        public Task<List<VoucherHistoryVM>> GetAllAsync();
        public Task<List<VoucherHistoryVM>> GetAllActiveAsync();
        public Task<VoucherHistoryVM> GetByIDAsync(Guid IDVoucher, Guid IDOrder);
        public Task<bool> CreateAsync(VoucherHistoryCreateVM request);
        public Task<bool> RemoveAsync(Guid IDVoucher, Guid IDOrder, Guid idUserdelete);
        public Task<bool> UpdateAsync(Guid IDVoucher, Guid IDOrder, VoucherHistoryUpdateVM request);
        public Task<IEnumerable<VoucherHistoryVM>> GetHistoryByVoucherIdAsync(Guid IDVoucher);
    }
}
