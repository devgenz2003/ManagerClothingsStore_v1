using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Colors;
using CHERRY.BUS.ViewModels.Sizes;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.Services._2_Implements
{
    public class SizesService : ISizesService
    {
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public SizesService(CHERRY_DBCONTEXT CHERRY_DBCONTEXT, IMapper mapper)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(SizesCreateVM request)
        {
            if (request != null)
            {
                var Obj = new Sizes()
                {
                    ID = Guid.NewGuid(),
                    Name = request.Name,
                    HexCode = request.HexCode,
                    Status = 1,
                };
                await _dbcontext.Sizes.AddRangeAsync(Obj);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<SizesVM>> GetAllActiveAsync()
        {
            var Obj = await _dbcontext.Sizes.Where(b => b.Status == 1).ToListAsync();
            return _mapper.Map<List<SizesVM>>(Obj);
        }

        public async Task<List<SizesVM>> GetAllAsync()
        {
            var Obj = await _dbcontext.Sizes.ToListAsync();
            return _mapper.Map<List<SizesVM>>(Obj);
        }

        public async Task<SizesVM> GetByIDAsync(Guid ID)
        {
            var Obj = await _dbcontext.Sizes.FindAsync(ID);
            return _mapper.Map<SizesVM>(Obj);
        }

        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbcontext.Sizes.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbcontext.Sizes.Attach(obj);
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

        public async Task<bool> UpdateAsync(Guid ID, SizesUpdateVM request)
        {
            var Obj = await _dbcontext.Sizes.FindAsync(ID);
            if (Obj == null)
            {
                return false;
            }

            Obj.Name = request.Name;
            Obj.HexCode = request.HexCode;
            Obj.Status = request.Status;

            _dbcontext.Sizes.Update(Obj);
            await _dbcontext.SaveChangesAsync();
            return true;
        }
    }
}
