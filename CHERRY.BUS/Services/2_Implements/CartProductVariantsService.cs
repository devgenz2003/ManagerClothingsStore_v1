using AutoMapper;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.CartProductVariants;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CHERRY.BUS.Services._2_Implements
{
    public class CartProductVariantsService : ICartProductVariantsService
    {
        private readonly CHERRY_DBCONTEXT _dbContext;
        private readonly IMapper _mapper;

        public CartProductVariantsService(IMapper mapper)
        {
            _dbContext = new CHERRY_DBCONTEXT();
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private async Task<List<CartProductVariantsVM>> GetCartProductVariantsVM(Expression<Func<CartProductVariants, bool>> predicate = null)
        {
            IQueryable<CartProductVariants> query = _dbContext.CartProductVariants;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            List<CartProductVariantsVM> items = await query
                .ProjectTo<CartProductVariantsVM>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return items;
        }
        public async Task<List<CartProductVariantsVM>> GetAllActiveAsync()
        {
            var list = await _dbContext.CartProductVariants
               .Where(c => c.Status != 0)
               .ProjectTo<CartProductVariantsVM>(_mapper.ConfigurationProvider)
               .ToListAsync();

            return list;
        }
        public async Task<List<CartProductVariantsVM>> GetAllAsync()
        {
            var list = await _dbContext.CartProductVariants
                           .ProjectTo<CartProductVariantsVM>(_mapper.ConfigurationProvider)
                           .ToListAsync();

            return list;
        }
        public async Task<CartProductVariantsVM> GetByIDAsync(Guid IDCart, Guid? IDOptions)
        {
            var cartClassifyItem = await _dbContext.CartProductVariants
                .Where(c => c.IDCart == IDCart && c.IDOptions == IDOptions)
                .FirstOrDefaultAsync();

            if (cartClassifyItem == null)
            {
                return null; // Hoặc xử lý khi không tìm thấy mục
            }

            var cartClassifyItemVM = _mapper.Map<CartProductVariantsVM>(cartClassifyItem);
            return cartClassifyItemVM;
        }
        public async Task<bool> RemoveAsync(Guid IDCart, Guid? IDOptions, Guid idUserdelete)
        {
            try
            {
                var cartItem = await _dbContext.CartProductVariants
                    .FirstOrDefaultAsync(c => c.IDCart == IDCart && c.IDOptions == IDOptions);

                if (cartItem != null)
                {
                    _dbContext.CartProductVariants.Remove(cartItem); // Xoá mục khỏi DbSet
                    await _dbContext.SaveChangesAsync(); // Áp dụng thay đổi vào CSDL

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
        public async Task<List<CartProductVariantsVM>> GetAllByCartIDAsync(Guid ID_Cart)
        {
            Expression<Func<CartProductVariants, bool>> predicate = c => c.IDCart == ID_Cart;
            return await GetCartProductVariantsVM(predicate);
        }
        public Task<bool> CreateAsync(CartProductVariantsCreateVM request)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(Guid ID_Cart, Guid? IDVariants, CartProductVariantsUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
