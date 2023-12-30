using CHERRY.DAL.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace CHERRY.DAL.Entities
{
    public class Order : EntityBase
    {
        public int HexCode { get; set; }
        public string? IDUser { get; set; } 
        public string CustomerName { get; set; } = null!; // Tên khách hàng
        public string CustomerPhone { get; set; } = null!; // Số điện thoại khách hàng
        public string CustomerEmail { get; set; } = null!;// Địa chỉ email khách hàng
        public string ShippingAddress { get; set; } = null!; 
        public string? ShippingAddressLine2 { get; set; }
        public DateTime ShipDate { get; set; } = DateTime.UtcNow.AddDays(3);
        public decimal TotalAmount { get; set; }
        public decimal? Cotsts { get; set; }
        public string? VoucherCode { get; set; }
        public string? Notes { get; set; }
        public bool TrackingCheck { get; set; } = false; //người dùng xáC NHẬN đơn hàng => sang hoá đơn
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
        //
        public virtual ICollection<OrderVariant> OrderVariant { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual VoucherHistory VoucherHistory { get; set; }
    }
    public enum PaymentMethod
    {
        ChuyenKhoanNganHang, // Chuyển khoản ngân hàng
        TienMatKhiGiaoHang,  // Tiền mặt khi giao hàng
    }
    public enum ShippingMethod
    {
        GiaoHangTieuChuan,   // Giao hàng tiêu chuẩn
        GiaoHangNhanh,       // Giao hàng nhanh
        GiaoHangTrongNgay,   // Giao hàng trong ngày
        NhanTaiCuaHang,      // Nhận tại cửa hàng
        GiaoHangQuocTe       // Giao hàng quốc tế
    }
    public enum PaymentStatus
    {
        Unknown,   // Chưa thanh toán
        Success,     // Đã thanh toán
        Pending,        // Đang xử lý
        Failure,    // Lỗi thanh toán
    }
    public enum OrderStatus
    {
        Pending,        //Chưa giải quyết,
        Processing,     //Xử lý
        Shipped,        //Đã vận chuyển
        Delivered,      //Đã giao hàng <=> success
        Cancelled,      //Đã hủy
        Returned ,       //Trả lại
    }
}
