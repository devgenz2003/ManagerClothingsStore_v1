using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Brand;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CHERRY.BUS.Services._2_Implements
{
    public class BrandServices : IBrandServices
    {
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public BrandServices(CHERRY_DBCONTEXT CHERRY_DBCONTEXT, IMapper mapper)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(BrandCreateVM request)
        {
            if (request != null)
            {
                var brand = new Brand()
                {
                    ID = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Status = request.Status,
                };
                await _dbcontext.Brand.AddRangeAsync(brand);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<BrandVM>> GetAllActiveAsync()
        {
            var activeBrands = await _dbcontext.Brand.Where(b => b.Status == 1).ToListAsync();
            return _mapper.Map<List<BrandVM>>(activeBrands);
        }

        public async Task<List<BrandVM>> GetAllAsync()
        {
            var brands = await _dbcontext.Brand.ToListAsync();
            return _mapper.Map<List<BrandVM>>(brands);
        }

        public async Task<BrandVM> GetByIDAsync(Guid ID)
        {
            var brand = await _dbcontext.Brand.FindAsync(ID);
            return _mapper.Map<BrandVM>(brand);
        }

        public async Task<bool> UpdateAsync(Guid ID, BrandUpdateVM request)
        {
            var brand = await _dbcontext.Brand.FindAsync(ID);
            if (brand == null)
            {
                return false;
            }

            brand.Name = request.Name;
            brand.Description = request.Description;
            brand.Status = request.Status;

            _dbcontext.Brand.Update(brand);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbcontext.Brand.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbcontext.Brand.Attach(obj);
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
    }
}
