using AutoMapper;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.BUS.ViewModels.PromotionVariants;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using static CHERRY.DAL.Entities.Promotion;
using static CHERRY.DAL.Entities.Voucher;

namespace CHERRY.BUS.Services._2_Implements
{
    public class PromotionService : IPromotionService
    {
        private readonly CHERRY_DBCONTEXT _dbContext;
        private readonly IMapper _mapper;

        public PromotionService(IMapper mapper)
        {
            _dbContext = new CHERRY_DBCONTEXT();
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<List<PromotionVariantsVM>> GetVariantsInPromotionAsync(Guid ID)
        {
            var specificPromotion = await _dbContext.Promotion
                
                  .FirstOrDefaultAsync(p => p.ID == ID && p.IsActive);
            if (specificPromotion == null)
            {
                return new List<PromotionVariantsVM>(); 
            }

            var variants = await _dbContext.PromotionVariant
                .Where(v => v.IDPromotion == ID)
                .ProjectTo<PromotionVariantsVM>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return variants;
        }
        public async Task<bool> ActivatePromotionAsync(Guid ID)
        {
            var promotion = await _dbContext.Promotion.FindAsync(ID);
            if (promotion == null) return false;

            promotion.IsActive = true;
            _dbContext.Promotion.Update(promotion);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<decimal> ApplyPromotionAsync(Guid ID, decimal originalPrice)
        {
            var promotion = await _dbContext.Promotion.FindAsync(ID);
            if (promotion == null) return originalPrice;

            switch (promotion.Type)
            {
                case Types.Percent:
                    return originalPrice - (originalPrice * promotion.DiscountAmount / 100);

                case Types.Cash:
                    return Math.Max(originalPrice - promotion.DiscountAmount, 0);

                default:
                    return originalPrice;
            }
        }
        public async Task<bool> CreateAsync(PromotionCreateVM request)
        {
            var newPromotion = new Promotion
            {
                SKU = request.SKU,
                Content = request.Content,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = request.IsActive,
                DiscountAmount = request.DiscountAmount,
                Type = request.Type,
                CreateBy = request.CreateBy,
                Status = request.Status
            };

            await _dbContext.Promotion.AddAsync(newPromotion);
            await _dbContext.SaveChangesAsync();
            foreach (var variantId in request.SelectedVariantIds)
            {
                var promotionVariant = new PromotionVariant
                {
                    IDVariant = variantId,
                    IDPromotion = newPromotion.ID,
                    Status = 1,
                    CreateBy = request.CreateBy
                };
                await _dbContext.PromotionVariant.AddAsync(promotionVariant);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeactivatePromotionAsync(Guid ID)
        {
            var promotion = await _dbContext.Promotion.FindAsync(ID);
            if (promotion == null) return false;

            promotion.IsActive = false;
            _dbContext.Promotion.Update(promotion);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<PromotionVM>> GetAllActiveAsync()
        {
            var promotions = await _dbContext.Promotion
                            .Where(p => p.IsActive)
                            .ToListAsync();
            return _mapper.Map<List<PromotionVM>>(promotions);
        }
        public async Task<List<PromotionVM>> GetAllAsync()
        {
            var promotions = await _dbContext.Promotion.ToListAsync();
            return _mapper.Map<List<PromotionVM>>(promotions);
        }
        public async Task<PromotionVM> GetByIDAsync(Guid ID)
        {
            var promotion = await _dbContext.Promotion.FindAsync(ID);
            return _mapper.Map<PromotionVM>(promotion);
        }
        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbContext.Promotion.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbContext.Promotion.Attach(obj);
                        await _dbContext.SaveChangesAsync();


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
        public async Task<bool> UpdateAsync(Guid ID, PromotionUpdateVM request)
        {
            var promotion = await _dbContext.Promotion.FindAsync(ID);
            if (promotion == null) return false;

            _mapper.Map(request, promotion);
            _dbContext.Promotion.Update(promotion);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ValidatePromotionAsync(Guid ID)
        {
            var promotion = await _dbContext.Promotion.FindAsync(ID);
            if (promotion == null) return false;

            return true;
        }
    }
}
