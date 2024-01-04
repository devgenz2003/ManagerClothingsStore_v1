using AutoMapper;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.CategoriesVariants;
using CHERRY.BUS.ViewModels.PromotionVariants;
using CHERRY.DAL.ApplicationDBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.Services._2_Implements
{
    public class PromotionVariantService : IPromotionVariantService
    {
        private readonly CHERRY_DBCONTEXT _dbContext;
        private readonly IMapper _mapper;
        public PromotionVariantService(IMapper mapper)
        {
            _dbContext = new CHERRY_DBCONTEXT();
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<bool> CreateAsync(PromotionVariantsCreateVM request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PromotionVariantsVM>> GetAllActiveAsync()
        {
            try
            {
                return _dbContext.PromotionVariant.AsQueryable().Where(c => c.Status != 0)
                    .ProjectTo<PromotionVariantsVM>(_mapper.ConfigurationProvider).ToList();
            }
            catch
            {
                return null;
            }
        }

        public Task<List<PromotionVariantsVM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PromotionVariantsVM> GetByIDAsync(Guid IDVariant, Guid IDPromotion)
        {
            var PromotionVariants = await _dbContext.PromotionVariant
                .Where(c => c.IDVariant == IDVariant && c.IDPromotion == IDPromotion)
                .FirstOrDefaultAsync();

            if (PromotionVariants == null)
            {
                return null; 
            }

            var PromotionVariantsvm = _mapper.Map<PromotionVariantsVM>(PromotionVariants);
            return PromotionVariantsvm;
        }

        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Guid ID, PromotionVariantsUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
