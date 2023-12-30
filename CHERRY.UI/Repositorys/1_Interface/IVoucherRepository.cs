using CHERRY.BUS.ViewModels.Voucher;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface IVoucherRepository
    {
        public Task<List<VoucherVM>> GetAllAsync();
        public Task<List<VoucherVM>> GetAllActiveAsync();
        public Task<VoucherVM> GetByIDAsync(Guid ID);
        public Task<List<VoucherVM>> GetVoucherByUser(string IDUser);
        public Task<bool> CreateAsync(VoucherCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, VoucherUpdateVM request);
        public Task<List<VoucherVM>> SearchVouchersAsync(string keyword);
        public Task<bool> ActivateVoucherAsync(Guid ID);
        public Task<bool> DeactivateVoucherAsync(Guid ID);
        public Task<List<VoucherVM>> GetVouchersByExpirationDateAsync(DateTime expirationDate);
    }
}
