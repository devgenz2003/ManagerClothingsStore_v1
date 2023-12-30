using AutoMapper;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Category;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CHERRY.BUS.Services._2_Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public CategoryService(CHERRY_DBCONTEXT dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<bool> CreateAsync(CategoryCreateVM request)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var categoryCreateVM = new Categories()
                    {
                        ID = Guid.NewGuid(),
                        Name = request.Name,
                        Description = request.Description,
                        Status = 1,
                        CreateBy = request.CreateBy
                    };
                    await _dbcontext.Categories.AddAsync(categoryCreateVM);

                    await _dbcontext.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public async Task<List<CategoryVM>> GetAllActiveAsync()
        {
            var list = await _dbcontext.Categories.Where(c => c.Status != 0)
                                        .ProjectTo<CategoryVM>(_mapper.ConfigurationProvider).ToListAsync();
            return list;
        }
        public async Task<List<CategoryVM>> GetAllAsync()
        {
            var list = await _dbcontext.Categories.ProjectTo<CategoryVM>(_mapper.ConfigurationProvider).ToListAsync();
            return list;
        }
        public async Task<CategoryVM> GetByIDAsync(Guid ID)
        {
            var obj = await _dbcontext.Categories.AsQueryable().SingleOrDefaultAsync(c => c.ID == ID);
            var objVM = _mapper.Map<CategoryVM>(obj);

            return objVM;
        }
        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbcontext.Categories.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbcontext.Categories.Attach(obj);
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
        public async Task<bool> UpdateAsync(Guid ID, CategoryUpdateVM request)
        {
            try
            {
                var listObj = await _dbcontext.Categories.ToListAsync();
                var objForUpdate = listObj.FirstOrDefault(c => c.ID == ID);
                objForUpdate.Name = request.CategoryName;
                objForUpdate.Description = request.Description;

                objForUpdate.Status = request.Status;
                objForUpdate.ModifieBy = request.ModifieBy;
                _dbcontext.Categories.Attach(objForUpdate);
                await Task.FromResult<Categories>(_dbcontext.Categories.Update(objForUpdate).Entity);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
