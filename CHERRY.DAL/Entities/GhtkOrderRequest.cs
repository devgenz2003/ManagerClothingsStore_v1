using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class GhtkOrderRequest : EntityBase
    {
        public Guid IDOrder { get; set; }
        public string? PickAddress { get; set; }//Địa chỉ ngắn gọn để lấy nhận hàng hóa. Ví dụ: nhà số 5, tổ 3, ngách 11, ngõ 45
        public string PickProvince { get; set; } = null!;//Tên tỉnh/thành phố nơi lấy hàng hóa
        public string PickDistrict { get; set; } = null!;
        public string? PickWard { get; set; }
        public string? PickStreet { get; set; }
        public string? Address { get; set; }
        public string Province { get; set; } = null!;
        public string District { get; set; } = null!;
        public string? Ward { get; set; }
        public string? Street { get; set; }
        public int Weight { get; set; }
        public int? Value { get; set; }
        public string? Transport { get; set; }
        public string DeliverOption { get; set; } = null!;

    }
}
