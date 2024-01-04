using AutoMapper;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.BUS.ViewModels.PromotionVariants;
using CHERRY.BUS.ViewModels.Variants;
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
            var promotionVariants = await _dbContext.PromotionVariant
                .Where(pv => pv.IDPromotion == ID)
                .ToListAsync();

            foreach (var promotionVariant in promotionVariants)
            {
                var options = await _dbContext.Options
                    .Where(opt => opt.IDVariant == promotionVariant.IDVariant)
                    .ToListAsync();

                var promotion = await _dbContext.Promotion.FindAsync(ID);

                if (promotion != null)
                {
                    foreach (var option in options)
                    {
                        decimal discountAmount = 0;

                        switch (promotion.Type)
                        {
                            case Types.Percent:
                                discountAmount = option.RetailPrice * promotion.DiscountAmount / 100;
                                break;

                            case Types.Cash:
                                discountAmount = Math.Min(option.RetailPrice, promotion.DiscountAmount);
                                break;
                        }

                        option.DiscountedPrice = discountAmount;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();

            return originalPrice;
        }
        public async Task<bool> CreateAsync(PromotionCreateVM request)
        {
            foreach (var variantId in request.SelectedVariantIds)
            {
                var existingPromotionVariant = await _dbContext.PromotionVariant
                    .FirstOrDefaultAsync(pv => pv.IDVariant == variantId && pv.Status == 1);

                if (existingPromotionVariant != null)
                {
                    existingPromotionVariant.Status = 0; 
                }
            }
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
            foreach (var variantId in request.SelectedVariantIds)
            {
                var optionsToUpdate = await _dbContext.Options.Where(opt => opt.IDVariant == variantId).ToListAsync();

                foreach (var optionToUpdate in optionsToUpdate)
                {
                    var discountedPrice = await ApplyPromotionAsync(newPromotion.ID, optionToUpdate.RetailPrice);
                    optionToUpdate.DiscountedPrice = discountedPrice;
                }
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
