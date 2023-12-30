
using AutoMapper;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.OrderVariant;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CHERRY.BUS.Services._2_Implements
{
    public class OrderVariantService : IOrderVariantService
    {
        private readonly CHERRY_DBCONTEXT _dbContext;
        private readonly IMapper _mapper;
        public OrderVariantService(CHERRY_DBCONTEXT dbcontext, IMapper mapper)
        {
            _dbContext = dbcontext;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<bool> CreateAsync(OrderVariantCreateVM request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var order = await _dbContext.Order.FirstOrDefaultAsync(c => c.ID == request.IDOrder);

                if (order == null)
                {
                    return false;
                }

                var orderDetailsList = new List<OrderVariant>();
                decimal totalAmount = 0;

                var options = await _dbContext.Options.FirstOrDefaultAsync(v => v.ID == request.IDOptions);

                if (options != null)
                {
                    var orderDetail = new OrderVariant
                    {
                        ID = Guid.NewGuid(),
                        IDOrder = order.ID,
                        IDOptions = request.IDOptions,
                        Quantity = request.Quantity,
                        UnitPrice = options.RetailPrice,
                        TotalAmount = request.Quantity * options.RetailPrice,
                        Status = 1
                    };

                    orderDetailsList.Add(orderDetail);
                    decimal discountValue = request.Discount ?? 0;
                    totalAmount += (orderDetail.UnitPrice * orderDetail.Quantity) - discountValue;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                bool stockUpdated = await CheckAndReduceStock(request.IDOptions, request.Quantity);
                if (!stockUpdated)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                order.TotalAmount = totalAmount;

                _dbContext.OrderVariant.AddRange(orderDetailsList);

                await _dbContext.SaveChangesAsync();

                await UpdateTotalAmountForOrder(order.ID);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        private async Task<bool> CheckAndReduceStock(Guid IDOptions, int quantity)
        {
            if (IDOptions != Guid.Empty)
            {
                var optionItem = await _dbContext.Options
                                        .FirstOrDefaultAsync(o => o.ID == IDOptions);

                if (optionItem != null && optionItem.StockQuantity >= quantity)
                {
                    optionItem.StockQuantity -= quantity;
                    await _dbContext.SaveChangesAsync(); 
                    return true;
                }
            }
            return false;
        }
        private async Task UpdateTotalAmountForOrder(Guid orderId)
        {
            var orderDetailsList = await _dbContext.OrderVariant
                .Where(od => od.IDOrder == orderId)
                .ToListAsync();

            if (orderDetailsList != null && orderDetailsList.Any())
            {
                decimal totalAmount = orderDetailsList.Sum(od => od.TotalAmount);

                var orderToUpdate = await _dbContext.Order.FirstOrDefaultAsync(o => o.ID == orderId);
                if (orderToUpdate != null)
                {
                    orderToUpdate.TotalAmount = totalAmount;
                    _dbContext.Entry(orderToUpdate).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
        public async Task<List<OrderVariantVM>> GetAllActiveAsync()
        {
            try
            {
                var activeOrderDetails = await _dbContext.OrderVariant
                    .Where(od => od.Status == 1)
                    .ToListAsync();

                var orderDetailVMs = _mapper.Map<List<OrderVariantVM>>(activeOrderDetails);

                return orderDetailVMs;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<OrderVariantVM>> GetAllAsync()
        {
            try
            {
                var orderDetails = await _dbContext.OrderVariant.ToListAsync();
                var orderDetailVMs = _mapper.Map<List<OrderVariantVM>>(orderDetails);

                return orderDetailVMs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<OrderVariantVM> GetByIDAsync(Guid ID)
        {
            try
            {
                var orderDetail = await _dbContext.OrderVariant.FindAsync(ID);

                if (orderDetail == null)
                {
                    return null;
                }

                var orderDetailVM = _mapper.Map<OrderVariantVM>(orderDetail);

                return orderDetailVM;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Task<bool> RemoveAsync(Guid ID, Guid IDUserdelete)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Guid ID, OrderVariantlUpdateVM request)
        {
            throw new NotImplementedException();
        }
    }
}
