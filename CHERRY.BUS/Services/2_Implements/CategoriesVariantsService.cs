using AutoMapper;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.CartProductVariants;
using CHERRY.BUS.ViewModels.CategoriesVariants;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CHERRY.BUS.Services._2_Implements
{
    public class CategoriesVariantsService : ICategoriesVariantsService
    {
        private readonly CHERRY_DBCONTEXT _dbContext;
        private readonly IMapper _mapper;
        public CategoriesVariantsService(IMapper mapper)
        {
            _dbContext = new CHERRY_DBCONTEXT();
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<bool> CreateAsync(CategoriesVariantsCreateVM request)
        {

            var obj = new CategoriesVariants()
            {
                CreateDate = DateTime.Now,
                IDCategories = request.IDCategories,
                IDVariants = request.IDVariants,
                Status = 1
            };

            await _dbContext.CategoriesVariants.AddAsync(obj);
            await _dbContext.SaveChangesAsync();


            return true;
        }
        public async Task<List<CategoriesVariantsVM>> GetAllActiveAsync()
        {
            try
            {
                return _dbContext.CategoriesVariants.AsQueryable().Where(c => c.Status != 0)
                    .ProjectTo<CategoriesVariantsVM>(_mapper.ConfigurationProvider).ToList();
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<CategoriesVariantsVM>> GetAllAsync()
        {
            try
            {
                return _dbContext.CategoriesVariants
                    .AsQueryable().ProjectTo<CategoriesVariantsVM>(_mapper.ConfigurationProvider).ToList();
            }
            catch
            {
                return null;
            }
        }
        public async Task<CategoriesVariantsVM> GetByIDAsync(Guid IDVariant, Guid IDCategory)
        {
            var CategoriesVariants = await _dbContext.CategoriesVariants
                .Where(c => c.IDCategories == IDCategory && c.IDVariants == IDVariant)
                .FirstOrDefaultAsync();

            if (CategoriesVariants == null)
            {
                return null; // Hoặc xử lý khi không tìm thấy mục
            }

            var CategoriesVariantsvm = _mapper.Map<CategoriesVariantsVM>(CategoriesVariants);
            return CategoriesVariantsvm;
        }
        public async Task<bool> RemoveAsync(Guid IDVariant, Guid IDCategory, string IDUserdelete)
        {
            try
            {
                var cartItem = await _dbContext.CategoriesVariants
                    .FirstOrDefaultAsync(c => c.IDVariants == IDVariant && c.IDCategories == IDCategory);

                if (cartItem != null)
                {
                    _dbContext.CategoriesVariants.Remove(cartItem); // Xoá mục khỏi DbSet
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
        public Task<bool> UpdateAsync(Guid ID, CategoriesVariantsUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
