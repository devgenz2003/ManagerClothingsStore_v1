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

                  .FirstOrDefaultAsync(p => p.ID == ID);
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
        public async Task<decimal> ApplyPromotionAsync(Guid promotionID, List<Guid> variantIDs, decimal originalPrice)
        {

            var promotion = await _dbContext.Promotion.FindAsync(promotionID);

            if (promotion != null)
            {
                foreach (var variantID in variantIDs)
                {
                    var options = await _dbContext.Options.Where(opt => opt.IDVariant == variantID).ToListAsync();

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

                await _dbContext.SaveChangesAsync();

                return originalPrice;
            }

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
                    var discountedPrice = await ApplyPromotionAsync(newPromotion.ID, new List<Guid> { variantId }, optionToUpdate.RetailPrice);

                    var discountHistory = new DiscountHistory
                    {
                        CreateBy = request.CreateBy,
                        Timestamp = DateTime.Now,
                        IDVariant = variantId,
                        PreviousDiscountedPrice = optionToUpdate.RetailPrice,
                        NewDiscountedPrice = optionToUpdate.DiscountedPrice.Value,
                        Status = 1
                    };

                    _dbContext.DiscountHistory.Add(discountHistory);
                    optionToUpdate.DiscountedPrice = discountedPrice;
                    _dbContext.Options.Update(optionToUpdate);
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

            var promotionVariants = await _dbContext.PromotionVariant
                .Where(pv => pv.IDPromotion == ID && pv.Status == 1)
                .ToListAsync();

            foreach (var pv in promotionVariants)
            {
                var variantOptions = await _dbContext.Options
                    .Where(opt => opt.IDVariant == pv.IDVariant)
                    .ToListAsync();

                foreach (var option in variantOptions)
                {
                    option.DiscountedPrice = 0; 
                    _dbContext.Options.Update(option);
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<PromotionVM>> GetAllActiveAsync()
        {
            var promotions = await _dbContext.Promotion
                  .Where(p => p.Status != 0)
                  .Include(p => p.PromotionVariants)
                  .ToListAsync();

            var mappedPromotions = promotions.Select(p =>
            {
                var promotionVM = _mapper.Map<PromotionVM>(p);
                promotionVM.IDVariant = p.PromotionVariants.Select(pv => pv.IDVariant).ToList();
                return promotionVM;
            }).ToList();

            return mappedPromotions;
        }
        public async Task<List<PromotionVM>> GetAllAsync()
        {
            var promotions = await _dbContext.Promotion
                  .Include(p => p.PromotionVariants)
                  .ToListAsync();

            var mappedPromotions = promotions.Select(p =>
            {
                var promotionVM = _mapper.Map<PromotionVM>(p);
                promotionVM.IDVariant = p.PromotionVariants.Select(pv => pv.IDVariant).ToList();
                return promotionVM;
            }).ToList();

            return mappedPromotions;
        }
        public async Task<PromotionVM> GetByIDAsync(Guid ID)
        {
            var promotion = await _dbContext.Promotion
         .Where(p => p.ID == ID)
         .Include(p => p.PromotionVariants)
         .FirstOrDefaultAsync();

            if (promotion == null)
            {
                return null;
            }

            var promotionVM = _mapper.Map<PromotionVM>(promotion);
            promotionVM.IDVariant = promotion.PromotionVariants.Select(pv => pv.IDVariant).ToList();

            return promotionVM;
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
            var promotion = await _dbContext.Promotion
                   .Include(p => p.PromotionVariants) 
                   .FirstOrDefaultAsync(v => v.ID == ID);

            if (promotion == null)
                return false;

            promotion.ModifieBy = request.ModifiedBy;
            promotion.SKU = request.SKU;
            promotion.Content = request.Content;
            promotion.StartDate = request.StartDate;
            promotion.EndDate = request.EndDate;
            promotion.IsActive = request.IsActive;
            promotion.DiscountAmount = request.DiscountAmount;
            promotion.Type = request.Type;
            promotion.Status = request.Status;
            if (promotion.PromotionVariants != null)
                _dbContext.PromotionVariant.RemoveRange(promotion.PromotionVariants);
            promotion.PromotionVariants = request.SelectedVariantIds
                .Select(id => new PromotionVariant
                {
                    IDVariant = id,
                    IDPromotion = promotion.ID,
                    CreateBy = "",
                    Status = 1
                }).ToList();

            _dbContext.Promotion.Update(promotion);
            await ApplyDiscountToPromotionVariants(promotion);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        private async Task ApplyDiscountToPromotionVariants(Promotion promotion)
        {
            foreach (var promotionVariant in promotion.PromotionVariants)
            {
                var optionsToUpdate = await _dbContext.Options
                    .Where(opt => opt.IDVariant == promotionVariant.IDVariant)
                    .ToListAsync();

                foreach (var optionToUpdate in optionsToUpdate)
                {
                    // Áp dụng giảm giá tại đây
                    decimal discountedPrice = CalculateDiscountedPrice(optionToUpdate.RetailPrice, promotion);

                    // Cập nhật giá giảm giá cho sản phẩm
                    optionToUpdate.DiscountedPrice = discountedPrice;
                }
            }
        }
        private decimal CalculateDiscountedPrice(decimal originalPrice, Promotion promotion)
        {
            decimal discountAmount = 0;

            switch (promotion.Type)
            {
                case Types.Percent:
                    discountAmount = originalPrice * promotion.DiscountAmount / 100;
                    break;

                case Types.Cash:
                    discountAmount = Math.Min(originalPrice, promotion.DiscountAmount);
                    break;
            }

            return discountAmount;
        }
        public async Task<bool> ValidatePromotionAsync(Guid ID)
        {
            var promotion = await _dbContext.Promotion.FindAsync(ID);
            if (promotion == null) return false;

            return true;
        }
    }
}
