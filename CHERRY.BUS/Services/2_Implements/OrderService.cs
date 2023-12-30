using AutoMapper;
using CHERRY.BUS.Services._1_Interface;
using CHERRY.BUS.ViewModels.Order;
using CHERRY.BUS.ViewModels.OrderVariant;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using static CHERRY.DAL.Entities.Voucher;

namespace CHERRY.BUS.Services._2_Implements
{
    public class OrderService : IOrderService
    {
        private readonly CHERRY_DBCONTEXT _dbContext;
        private readonly IMapper _mapper;
        public OrderService(CHERRY_DBCONTEXT dbcontext, IMapper mapper)
        {
            _dbContext = dbcontext;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<bool> ConfirmOrderAsync(Guid IDOrder, bool trackingCheck)
        {
            var order = await _dbContext.Order.FirstOrDefaultAsync(o => o.ID == IDOrder);

            if (order != null)
            {
                order.OrderStatus = OrderStatus.Delivered;

                if (trackingCheck)
                {
                    order.TrackingCheck = true;
                }

                var orderVariants = _dbContext.OrderVariant.Where(ov => ov.IDOrder == IDOrder).ToList();

                foreach (var orderVariant in orderVariants)
                {
                    orderVariant.HasPurchased = true;
                }

                _dbContext.Order.Update(order);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> CreateAsync(OrderCreateVM request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var order = _mapper.Map<Order>(request);
                order.ID = Guid.NewGuid();
                order.CreateBy = request.IDUser;
                order.IDUser = request.IDUser;
                order.CustomerName = request.CustomerName;
                order.CustomerPhone = request.CustomerPhone;
                order.CustomerEmail = request.CustomerEmail;
                order.OrderStatus = OrderStatus.Pending;
                order.ShipDate = request.ShipDate;
                order.ShippingAddress = request.ShippingAddress;
                order.ShippingAddressLine2 = request.ShippingAddressLine2;
                order.TrackingCheck = false;
                order.VoucherCode = request.VoucherCode;
                order.PaymentMethod = request.PaymentMethod;
                order.PaymentStatus = request.PaymentStatus;
                order.ShippingMethod = request.ShippingMethod;
                order.Status = 1;
                var orderDetailsList = new List<OrderVariant>();
                decimal totalAmount = 0;
                foreach (var directItem in request.OrderVariantCreateVM)
                {
                    var option = await _dbContext.Options.FirstOrDefaultAsync(c => c.ID == directItem.IDOptions);
                    if (option != null)
                    {
                        var ordervariant = new OrderVariant()
                        {
                            ID = Guid.NewGuid(),
                            IDOrder = order.ID,
                            IDOptions = option.ID,
                            UnitPrice = option.RetailPrice,
                            Quantity = directItem.Quantity,
                            TotalAmount = option.RetailPrice * directItem.Quantity,
                            Status = 1,
                            CreateBy  = option.CreateBy
                        };
                        orderDetailsList.Add(ordervariant);
                        decimal discountValue = directItem.Discount ?? 0;
                        totalAmount += (ordervariant.UnitPrice * ordervariant.Quantity) - discountValue;

                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                    bool stockUpdated = await CheckAndReduceStock(directItem.IDOptions, directItem.Quantity);
                    if (!stockUpdated)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.VoucherCode))
                {
                    var voucher = await _dbContext.Vouchers.FirstOrDefaultAsync(v => v.Code == request.VoucherCode);
                    if (voucher == null || !voucher.IsActive || voucher.Quantity <= 0)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    var discountAmount = CalculateDiscountAmount(voucher, totalAmount);
                    totalAmount -= discountAmount;

                    voucher.Quantity -= 1;
                    if (voucher.Quantity <= 0)
                    {
                        voucher.IsActive = false;
                    }
                    _dbContext.Vouchers.Update(voucher);

                    var voucherhistory = new VoucherHistory()
                    {
                        IDOrder = order.ID,
                        IDVoucher = voucher.ID,
                        Status = 1,
                        MoneyBeforReducition = totalAmount,
                        MoneyReducition = voucher.ReducedValue,
                        MoneyAfterReducition = totalAmount - discountAmount,
                        CreateBy = order.IDUser
                    };
                    _dbContext.VoucherHistory.Add(voucherhistory);
                }

                order.TotalAmount = totalAmount;
                _dbContext.Order.Add(order);
                _dbContext.OrderVariant.AddRange(orderDetailsList);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        private decimal CalculateDiscountAmount(Voucher voucher, decimal totalAmount)
        {
            if (voucher.Type == Types.Percent)
            {
                return (voucher.ReducedValue / 100m) * totalAmount;
            }
            if (voucher.Type == Types.Cash)
            {
                return voucher.ReducedValue;
            }

            return 0;
        }
        private async Task<bool> CheckAndReduceStock(Guid IDOptions, int quantity)
        {
            if (IDOptions != Guid.Empty && IDOptions != null)
            {
                var variantItem = await _dbContext.Options.FindAsync(IDOptions);
                if (variantItem != null)
                {
                    if (variantItem.StockQuantity < quantity)
                    {
                        return false;
                    }
                    variantItem.StockQuantity -= quantity;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
        public async Task<List<OrderVM>> GetAllActiveAsync()
        {
            var activeOrders = await _dbContext.Order
                           .Where(o => o.Status == 1)
                           .ToListAsync();

            var activeOrderVMs = _mapper.Map<List<OrderVM>>(activeOrders);

            return activeOrderVMs;
        }
        public async Task<List<OrderVM>> GetAllAsync()
        {
            var allOrders = await _dbContext.Order.ToListAsync();

            var allOrderVMs = _mapper.Map<List<OrderVM>>(allOrders);
            return allOrderVMs;
        }
        public async Task<List<OrderVM>> GetByCustomerIDAsync(string IDUser)
        {
            var orders = await _dbContext.Order
                               .Where(o => o.IDUser == IDUser && o.Status != 0)
                               .ToListAsync();

            return _mapper.Map<List<OrderVM>>(orders);
        }
        public async Task<List<OrderVM>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _dbContext.Order
                               .Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate)
                               .ToListAsync();

            return _mapper.Map<List<OrderVM>>(orders);
        }
        public async Task<OrderVM> GetByIDAsync(Guid ID)
        {
            var order = await _dbContext.Order.FirstOrDefaultAsync(o => o.ID == ID);
            return order != null ? _mapper.Map<OrderVM>(order) : null;
        }
        public async Task<List<OrderVM>> GetByStatusAsync(OrderStatus status)
        {
            var orders = await _dbContext.Order
                               .Where(o => o.OrderStatus == status)
                               .ToListAsync();

            return _mapper.Map<List<OrderVM>>(orders);
        }
        public async Task<List<OrderVariantVM>> GetOrderVariantByIDAsync(Guid IDOrder)
        {
            var orderDetails = await (
                 from p in _dbContext.Order
                 join dp in _dbContext.OrderVariant on p.ID equals dp.IDOrder
                 where dp.IDOrder == IDOrder
                 select new OrderVariantVM
                 {
                     ID = dp.ID,
                     IDOrder = p.ID,
                     IDOptions = dp.IDOptions,
                     HasPurchased = dp.HasPurchased,
                     HasReviewed = dp.HasReviewed,
                     ColorName = dp.Options.Color.Name,
                     SizeName = dp.Options.Sizes.Name,
                     VariantName = dp.Options.Variants.VariantName,
                     UnitPrice = dp.UnitPrice,
                     Discount = dp.Discount,
                     Quantity = dp.Quantity,
                     Status = dp.Status,
                     TotalAmount = dp.TotalAmount,
                 }
             ).ToListAsync();
            if (!orderDetails.Any())
            {
                return new List<OrderVariantVM>(); 
            }

            return orderDetails;
        }
        public async Task<bool> MarkAsCancelledAsync(Guid IDOrder)
        {
            var order = await _dbContext.Order
                                         .FirstOrDefaultAsync(o => o.ID == IDOrder);

            if (order == null || order.TrackingCheck)
            {
                return false;
            }
            order.OrderStatus = OrderStatus.Cancelled;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> MarkAsDeliveredAsync(Guid IDOrder)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var order = await _dbContext.Order
                  .FirstOrDefaultAsync(o => o.ID == IDOrder);
                if (order == null || order.TrackingCheck)
                {
                    return false;
                }
                if (order == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                order.OrderStatus = OrderStatus.Delivered;
                order.PaymentStatus = PaymentStatus.Success;
                // Tìm tất cả các OrderVariants liên quan và cập nhật trạng thái của chúng
                var orderVariants = await _dbContext.OrderVariant
                    .Where(ov => ov.IDOrder == IDOrder)
                    .ToListAsync();

                foreach (var orderVariant in orderVariants)
                {
                    orderVariant.HasPurchased = true;
                }
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        public async Task<bool> MarkAsReturnedAsync(Guid IDOrder)
        {
            var order = await _dbContext.Order
                               .FirstOrDefaultAsync(o => o.ID == IDOrder);
            if (order == null || order.TrackingCheck)
            {
                return false;
            }

            order.OrderStatus = OrderStatus.Returned;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> MarkAsShippedAsync(Guid IDOrder)
        {
            var order = await _dbContext.Order
                               .FirstOrDefaultAsync(o => o.ID == IDOrder);

            if (order == null || order.TrackingCheck)
            {
                return false;
            }
            order.OrderStatus = OrderStatus.Shipped;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbContext.Brand.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbContext.Brand.Attach(obj);
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
        public async Task<bool> UpdateAsync(Guid ID, OrderUpdateVM request)
        {
            var order = await _dbContext.Order
                               .FirstOrDefaultAsync(o => o.ID == ID);

            if (order == null || order.TrackingCheck)
            {
                return false;
            }
            _mapper.Map(request, order);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
