using CHERRY.BUS.ViewModels.VoucherUser;

namespace CHERRY.BUS.Services._1_Interfaces
{
    public interface IVoucherUserService
    {
        public Task<List<VoucherUserVM>> GetAllAsync();
        public Task<List<VoucherUserVM>> GetAllActiveAsync();
        public Task<VoucherUserVM> GetByIDAsync(Guid IDVoucher, string IDUser);
        public Task<bool> CreateAsync(VoucherUserCreateVM request);
        public Task<bool> RemoveAsync(Guid IDVoucher, string IDUsers, Guid idUserdelete);
        public Task<bool> UpdateAsync(Guid IDVoucher, string IDUsers, VoucherUserUpdateVM request);
    }
}
