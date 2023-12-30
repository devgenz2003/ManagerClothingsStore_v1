using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Colors;
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
    public class ColorsService : IColorsService
    {
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        public ColorsService(CHERRY_DBCONTEXT CHERRY_DBCONTEXT, IMapper mapper)
        {
            _dbcontext = CHERRY_DBCONTEXT;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(ColorsCreateVM request)
        {
            if (request != null)
            {
                var Obj = new Colors()
                {
                    ID = Guid.NewGuid(),
                    Name = request.Name,
                    HexCode = request.HexCode,
                    Status = 1,
                };
                await _dbcontext.Colors.AddRangeAsync(Obj);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ColorsVM>> GetAllActiveAsync()
        {
            var Obj = await _dbcontext.Colors.Where(b => b.Status == 1).ToListAsync();
            return _mapper.Map<List<ColorsVM>>(Obj);
        }

        public async Task<List<ColorsVM>> GetAllAsync()
        {
            var Obj = await _dbcontext.Colors.ToListAsync();
            return _mapper.Map<List<ColorsVM>>(Obj);
        }

        public async Task<ColorsVM> GetByIDAsync(Guid ID)
        {
            var Obj = await _dbcontext.Colors.FindAsync(ID);
            return _mapper.Map<ColorsVM>(Obj);
        }

        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbcontext.Colors.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbcontext.Colors.Attach(obj);
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

        public async Task<bool> UpdateAsync(Guid ID, ColorsUpdateVM request)
        {
            var Obj = await _dbcontext.Colors.FindAsync(ID);
            if (Obj == null)
            {
                return false;
            }

            Obj.Name = request.Name;
            Obj.HexCode = request.HexCode;
            Obj.Status = request.Status;

            _dbcontext.Colors.Update(Obj);
            await _dbcontext.SaveChangesAsync();
            return true;
        }
    }
}
