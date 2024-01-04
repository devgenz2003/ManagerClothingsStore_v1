using CHERRY.BUS.ViewModels.Order;
using CHERRY.BUS.ViewModels.OrderVariant;
using CHERRY.DAL.Entities;

namespace CHERRY.UI.Repositorys._1_Interface
{
    public interface IOrderRepository
    {
        public Task<List<OrderVM>> GetAllAsync();
        public Task<List<OrderVM>> GetAllActiveAsync();
        public Task<OrderVM> GetByIDAsync(Guid ID);
        public Task<List<OrderVariantVM>> GetOrderVariantByIDAsync(Guid IDOrder);
        public Task<bool> CreateAsync(OrderCreateVM request);
        public Task<bool> RemoveAsync(Guid ID, string IDUserdelete);
        public Task<bool> UpdateAsync(Guid ID, OrderUpdateVM request);
        // Lấy danh sách đơn hàng theo trạng thái
        public Task<List<OrderVM>> GetByStatusAsync(OrderStatus status);
        // Lấy danh sách đơn hàng trong một khoảng thời gian cụ thể
        public Task<List<OrderVM>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        // Lấy danh sách đơn hàng theo ID khách hàng
        public Task<List<OrderVM>> GetByCustomerIDAsync(string IDUser);
        // Đánh dấu một đơn hàng là đã giao hàng
        public Task<bool> MarkAsPaymentSuccessAsync(string HexCode);
        // Đánh dấu một đơn hàng là đã hoàn thành
        public Task<bool> MarkAsDeliveredAsync(Guid IDOrder);
        // Đánh dấu một đơn hàng là đã bị hủy
        public Task<bool> MarkAsCancelledAsync(Guid IDOrder);
        // Đánh dấu một đơn hàng là đã được trả lại
        public Task<bool> MarkAsReturnedAsync(Guid IDOrder);
        // Xác nhận đơn hàng với trạng thái theo dõi
        public Task<bool> ConfirmOrderAsync(Guid IDOrder, bool trackingCheck);

    }
}
