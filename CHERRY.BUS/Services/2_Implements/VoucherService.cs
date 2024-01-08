using AutoMapper;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.PromotionVariants;
using CHERRY.BUS.ViewModels.Voucher;
using CHERRY.BUS.ViewModels.VoucherUser;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
namespace CHERRY.BUS.Services._2_Implements
{
    public class VoucherService : IVoucherService
    {
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public VoucherService(CHERRY_DBCONTEXT CHERRY_DBCONTEXT, IMapper mapper)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _mapper = mapper;
        }
        public async Task<bool> ActivateVoucherAsync(Guid ID)
        {
            var voucher = await _dbcontext.Vouchers.FindAsync(ID);
            if (voucher == null)
            {
                return false;
            }

            voucher.IsActive = true;
            _dbcontext.Vouchers.Update(voucher);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
        public async Task<bool> CreateAsync(VoucherCreateVM request)
        {
            try
            {
                var newVoucher = new Voucher
                {
                    Code = request.Code,
                    Name = request.Name,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Quantity = request.Quantity,
                    Type = request.Type,
                    Key = request.Key,
                    MinimumAmount = request.MinimumAmount,
                    ReducedValue = request.ReducedValue,
                    IsActive = request.IsActive,
                    Status = 1,
                    CreateBy = request.CreateBy,
                };
                _dbcontext.Vouchers.Add(newVoucher);
                await _dbcontext.SaveChangesAsync();

                foreach (var userid in request.SelectedUser)
                {
                    var voucheruser = new VoucherUser
                    {
                        IDUser = userid,
                        IDVoucher = newVoucher.ID,
                        Status = 1,
                        CreateBy = request.CreateBy
                    };
                    await _dbcontext.VoucherUser.AddAsync(voucheruser);
                }
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeactivateVoucherAsync(Guid ID)
        {
            var voucher = await _dbcontext.Vouchers.FindAsync(ID);
            if (voucher == null)
            {
                return false; // Voucher không tìm thấy
            }

            voucher.IsActive = false; // Giả sử 'IsActive' là trường trạng thái kích hoạt
            _dbcontext.Vouchers.Update(voucher);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
        public async Task<List<VoucherVM>> GetAllActiveAsync()
        {
            var obj = await _dbcontext.Vouchers
                         .Where(o => o.Status == 1)
                         .ToListAsync();

            var objVMs = _mapper.Map<List<VoucherVM>>(obj);

            return objVMs;
        }
        public async Task<List<VoucherVM>> GetAllAsync()
        {
            var allOrders = await _dbcontext.Order.ToListAsync();

            var allOrderVMs = _mapper.Map<List<VoucherVM>>(allOrders);
            return allOrderVMs;

        }
        public async Task<VoucherVM> GetByIDAsync(Guid ID)
        {
            var obj = await _dbcontext.Vouchers.FindAsync(ID);
            if (obj == null)
            {
                return null; // Hoặc xử lý theo cách khác nếu không tìm thấy
            }

            return _mapper.Map<VoucherVM>(obj);
        }
        public async Task<List<VoucherUserVM>> GetUserInPromotionAsync(Guid ID)
        {
            var specificPromotion = await _dbcontext.Vouchers

                              .FirstOrDefaultAsync(p => p.ID == ID && p.IsActive);
            if (specificPromotion == null)
            {
                return new List<VoucherUserVM>();
            }

            var variants = await _dbcontext.VoucherUser
                .Where(v => v.IDVoucher == ID && v.Status != 0)
                .ProjectTo<VoucherUserVM>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return variants;
        }

        public async Task<List<VoucherVM>> GetVoucherByUser(string IDUser)
        {
            var datavoucher = await _dbcontext.VoucherUser
                .Where(c => c.IDUser == IDUser)
                .Select(c => c.Voucher)
                .Select(v => new VoucherVM
                {
                    ID = v.ID,
                    Code = v.Code,
                    Name = v.Name,
                    StartDate = v.StartDate,
                    EndDate = v.EndDate,
                    Quantity = v.Quantity,
                    Type = v.Type,
                    MinimumAmount = v.MinimumAmount,
                    ReducedValue = v.ReducedValue,
                    IsActive = v.IsActive,
                    Key = v.Key,
                    Status = (DateTime.Now >= v.StartDate && DateTime.Now <= v.EndDate && v.Quantity > 0) ? 1 : 0
                })
                .ToListAsync();
            return datavoucher;
        }
        public async Task<List<VoucherVM>> GetVouchersByExpirationDateAsync(DateTime expirationDate)
        {
            var obj = await _dbcontext.Vouchers
                                   .Where(v => v.EndDate == expirationDate)
                                   .ToListAsync();

            return _mapper.Map<List<VoucherVM>>(obj);
        }
        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbcontext.Vouchers.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbcontext.Vouchers.Attach(obj);
                        await _dbcontext.SaveChangesAsync();


                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public async Task<List<VoucherVM>> SearchVouchersAsync(string keyword)
        {
            var vouchers = await _dbcontext.Vouchers
                                    .Where(v => v.Name.Contains(keyword) || v.Key.Contains(keyword))
                                    .ToListAsync();

            return _mapper.Map<List<VoucherVM>>(vouchers);
        }
        public async Task<bool> UpdateAsync(Guid ID, VoucherUpdateVM request)
        {
            var voucher = await _dbcontext.Vouchers.FindAsync(ID);
            if (voucher == null)
            {
                return false; // Không tìm thấy voucher
            }

            // Cập nhật các trường của voucher
            voucher.Name = request.Name;
            voucher.Key = request.Key;
            // Cập nhật các trường khác...

            _dbcontext.Vouchers.Update(voucher);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
    }
}
