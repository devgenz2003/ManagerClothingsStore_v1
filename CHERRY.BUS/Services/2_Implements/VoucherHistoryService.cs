using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.VoucherHistory;
using CHERRY.BUS.ViewModels.VoucherUser;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CHERRY.BUS.Services._2_Implements
{
    public class VoucherHistoryService : IVoucherHistoryService
    {
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public VoucherHistoryService(CHERRY_DBCONTEXT CHERRY_DBCONTEXT, IMapper mapper)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(VoucherHistoryCreateVM request)
        {
            var obj = new VoucherHistory()
            {
                CreateDate = DateTime.Now,
                IDOrder = request.IDOrder,
                IDVoucher = request.IDVoucher,
                Status = 1
            };

            await _dbcontext.VoucherHistory.AddAsync(obj);
            await _dbcontext.SaveChangesAsync();


            return true;
        }

        public async Task<List<VoucherHistoryVM>> GetAllActiveAsync()
        {
            var obj = await _dbcontext.VoucherHistory
                             .Where(vu => vu.Status != 0)
                             .ToListAsync();

            return _mapper.Map<List<VoucherHistoryVM>>(obj);
        }

        public async Task<List<VoucherHistoryVM>> GetAllAsync()
        {
            var obj = await _dbcontext.VoucherHistory
                                         .ToListAsync();

            return _mapper.Map<List<VoucherHistoryVM>>(obj);
        }

        public async Task<VoucherHistoryVM> GetByIDAsync(Guid IDVoucher, Guid IDOrder)
        {
            var obj = await _dbcontext.VoucherHistory
                                      .Where(c => c.IDVoucher == IDVoucher && c.IDOrder == IDOrder)
                                      .FirstOrDefaultAsync();

            if (obj == null)
            {
                return null; // Hoặc xử lý khi không tìm thấy mục
            }

            var objVM = _mapper.Map<VoucherHistoryVM>(obj);
            return objVM;
        }

        public async Task<IEnumerable<VoucherHistoryVM>> GetHistoryByVoucherIdAsync(Guid IDVoucher)
        {
            var historyList = await _dbcontext.VoucherHistory
                                                .Where(vh => vh.IDVoucher == IDVoucher)
                                                .ToListAsync();

            var historyVMList = _mapper.Map<IEnumerable<VoucherHistoryVM>>(historyList);
            return historyVMList;
        }

        public async Task<bool> RemoveAsync(Guid IDVoucher, Guid IDOrder, Guid idUserdelete)
        {
            try
            {
                var obj = await _dbcontext.VoucherHistory
                    .FirstOrDefaultAsync(c => c.IDVoucher == IDVoucher && c.IDOrder == IDOrder);

                if (obj != null)
                {
                    _dbcontext.VoucherHistory.Remove(obj); // Xoá mục khỏi DbSet
                    await _dbcontext.SaveChangesAsync(); // Áp dụng thay đổi vào CSDL

                    return true;
                }
                else
                {
                    return false; // Không tìm thấy mục để xóa
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Guid IDVoucher, Guid IDOrder, VoucherHistoryUpdateVM request)
        {

            var voucherUser = await _dbcontext.VoucherHistory
                    .FirstOrDefaultAsync(vu => vu.IDVoucher == IDVoucher && vu.IDOrder == IDOrder);

            if (voucherUser == null)
            {
                return false; // Không tìm thấy liên kết để cập nhật
            }

            // Cập nhật các trường của voucherUser
            voucherUser.IDVoucher = request.IDVoucher;
            voucherUser.IDOrder = request.IDOrder;
            voucherUser.Status = request.Status;
            // Cập nhật các trường khác dựa trên cấu trúc của VoucherUserUpdateVM

            _dbcontext.VoucherHistory.Update(voucherUser);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
    }
}
