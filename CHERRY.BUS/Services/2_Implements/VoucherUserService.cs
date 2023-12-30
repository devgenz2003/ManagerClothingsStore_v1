using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.VoucherUser;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CHERRY.BUS.Services._2_Implements
{
    public class VoucherUserService : IVoucherUserService
    {
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public VoucherUserService(CHERRY_DBCONTEXT CHERRY_DBCONTEXT, IMapper mapper)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(VoucherUserCreateVM request)
        {
            var obj = new VoucherUser()
            {
                CreateDate = DateTime.Now,
                IDUser = request.IDUser,
                IDVoucher = request.IDVoucher,
                Status = 1
            };

            await _dbcontext.VoucherUser.AddAsync(obj);
            await _dbcontext.SaveChangesAsync();


            return true;
        }

        public async Task<List<VoucherUserVM>> GetAllActiveAsync()
        {
            var activeVoucherUsers = await _dbcontext.VoucherUser
                   .Where(vu => vu.Status != 0) 
                   .ToListAsync();

            return _mapper.Map<List<VoucherUserVM>>(activeVoucherUsers);
        }

        public async Task<List<VoucherUserVM>> GetAllAsync()
        {
            var activeVoucherUsers = await _dbcontext.VoucherUser
                               .ToListAsync();

            return _mapper.Map<List<VoucherUserVM>>(activeVoucherUsers);
        }

        public async Task<VoucherUserVM> GetByIDAsync(Guid IDVoucher, string IDUser)
        {
            var obj = await _dbcontext.VoucherUser
                           .Where(c => c.IDVoucher == IDVoucher && c.IDUser == IDUser)
                           .FirstOrDefaultAsync();

            if (obj == null)
            {
                return null; // Hoặc xử lý khi không tìm thấy mục
            }

            var objVM = _mapper.Map<VoucherUserVM>(obj);
            return objVM;
        }

        public async Task<bool> RemoveAsync(Guid IDVoucher, string IDUsers, Guid idUserdelete)
        {
            try
            {
                var obj = await _dbcontext.VoucherUser
                    .FirstOrDefaultAsync(c => c.IDVoucher == IDVoucher && c.IDUser == IDUsers);

                if (obj != null)
                {
                    _dbcontext.VoucherUser.Remove(obj); // Xoá mục khỏi DbSet
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

        public async Task<bool> UpdateAsync(Guid IDVoucher, string IDUsers, VoucherUserUpdateVM request)
        {
            var voucherUser = await _dbcontext.VoucherUser
                    .FirstOrDefaultAsync(vu => vu.IDVoucher == IDVoucher && vu.IDUser == IDUsers);

            if (voucherUser == null)
            {
                return false; // Không tìm thấy liên kết để cập nhật
            }

            // Cập nhật các trường của voucherUser
            voucherUser.IDVoucher = request.IDVoucher;
            voucherUser.IDUser = request.IDUser;
            voucherUser.Status = request.Status;
            // Cập nhật các trường khác dựa trên cấu trúc của VoucherUserUpdateVM

            _dbcontext.VoucherUser.Update(voucherUser);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
    }
}
