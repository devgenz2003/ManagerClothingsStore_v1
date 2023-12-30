using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Material;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
namespace CHERRY.BUS.Services._2_Implements
{
    public class MaterialServices : IMaterialServices
    {
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public MaterialServices(CHERRY_DBCONTEXT CHERRY_DBCONTEXT, IMapper mapper)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(MaterialCreateVM request)
        {
            if (request != null)
            {
                var material = new Material()
                {
                    ID = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Status = request.Status,
                };
                await _dbcontext.Material.AddRangeAsync(material);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<MaterialVM>> GetAllActiveAsync()
        {
            var materials = await _dbcontext.Material.Where(m => m.Status == 1).ToListAsync();
            return _mapper.Map<List<MaterialVM>>(materials);
        }
        public async Task<List<MaterialVM>> GetAllAsync()
        {
            var materials = await _dbcontext.Material.ToListAsync();
            return _mapper.Map<List<MaterialVM>>(materials);
        }
        public async Task<MaterialVM> GetByIDAsync(Guid ID)
        {
            var material = await _dbcontext.Material.FirstOrDefaultAsync(m => m.ID == ID);
            return _mapper.Map<MaterialVM>(material);
        }
        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbcontext.Material.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbcontext.Material.Attach(obj);
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
        public async Task<bool> UpdateAsync(Guid ID, MaterialUpdateVM request)
        {
            var material = await _dbcontext.Material.FirstOrDefaultAsync(m => m.ID == ID);
            if (material != null)
            {
                material.Name = request.Name;
                material.Description = request.Description;
                material.Status = request.Status;

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
