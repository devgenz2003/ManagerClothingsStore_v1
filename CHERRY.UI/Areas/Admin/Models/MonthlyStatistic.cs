namespace CHERRY.UI.Areas.Admin.Models
{
    public class MonthlyStatistic
    {
        public DateTime Month { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit => TotalRevenue - TotalCost;
        public int TotalOrder { get; set; }
        public int TotalOrdersnosuccess { get; set; }
        public int TotalOrderssuccess { get; set; }
        public MonthlyStatistic()
        {
            
        }
        public static decimal CalculatePercentageChange(MonthlyStatistic currentMonth, MonthlyStatistic previousMonth)
        {
            if (currentMonth == null)
            {
                throw new ArgumentNullException(nameof(currentMonth), "Thống kê của tháng hiện tại không được phép là null.");
            }

            if (previousMonth == null || previousMonth.TotalProfit == 0)
            {
                throw new InvalidOperationException("Lợi nhuận của tháng trước không được bằng 0 hoặc null khi tính tỷ lệ phần trăm.");
            }

            return ((currentMonth.TotalProfit - previousMonth.TotalProfit) / previousMonth.TotalProfit) * 100;
        }

    }
}
