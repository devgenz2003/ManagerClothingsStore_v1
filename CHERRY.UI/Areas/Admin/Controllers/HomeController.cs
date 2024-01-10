using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using CHERRY.UI.Areas.Admin.Models;
using CHERRY.Utilities;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CHERRY.UI.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    //[Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly CHERRY_DBCONTEXT _dbContext;
        private int totalOrders;
        private int totalOrdersnosuccess;
        private int totalOrderssuccess;
        private int TotalQuantityOptions;
        private decimal totalRevenue = 0M;
        private decimal totalCost = 0M;

        public HomeController(CHERRY_DBCONTEXT CHERRY_DBCONTEXT)
        {
            _dbContext = CHERRY_DBCONTEXT;
        }
        private async Task<MonthlyStatistic> CalculateStatistics(string month)
        {
            DateTime selectedMonth = string.IsNullOrEmpty(month) ? DateTime.Now : DateTime.Parse(month);

            var startDate = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var bankPaymentOrders = await GetBankPaymentOrders(selectedMonth);
            var totalStockQuantity = await _dbContext.Options
                .SumAsync(option => option.StockQuantity);
            ViewBag.TotalQuantityOptions = totalStockQuantity;

            decimal totalBankPayments = bankPaymentOrders.Sum(order => order.TotalAmount);
            ViewBag.BankPaymentOrders = bankPaymentOrders;
            ViewBag.TotalBankPayments = totalBankPayments;

            var orders = _dbContext.Order.Where(o => o.CreateDate >= startDate && o.CreateDate <= endDate && o.OrderStatus == OrderStatus.Delivered);

            if (await orders.AnyAsync())
            {
                totalRevenue = await orders.SumAsync(o => o.TotalAmount);
            }
            totalRevenue = Math.Max(0, totalRevenue);

            var expenses = _dbContext.Options.Where(e => e.CreateDate >= startDate && e.CreateDate <= endDate);

            if (await expenses.AnyAsync())
            {
                totalCost = await expenses
                       .Select(e => e.CostPrice * e.StockQuantity)
                       .SumAsync();
            }
            totalCost = Math.Max(0, totalCost);

            totalOrders = await _dbContext.Order
                    .Where(order => order.CreateDate >= startDate && order.CreateDate <= endDate)
                    .CountAsync();

            totalOrdersnosuccess = await _dbContext.Order
                   .Where(order => order.CreateDate >= startDate && order.CreateDate <= endDate && order.OrderStatus != OrderStatus.Delivered)
                   .CountAsync();

            totalOrderssuccess = await _dbContext.Order
                   .Where(order => order.CreateDate >= startDate && order.CreateDate <= endDate && order.OrderStatus == OrderStatus.Delivered)
                   .CountAsync();
            var bestSellingProducts = await CalculateBestSellingProducts(startDate, endDate);
            return new MonthlyStatistic
            {
                TotalOrdersnosuccess = totalOrdersnosuccess,
                TotalOrder = totalOrders,
                Month = startDate,
                TotalRevenue = totalRevenue,
                BestSellingProducts = bestSellingProducts,
                TotalCost = totalCost,
                TotalOrderssuccess = totalOrderssuccess
            };
        }
        private async Task<int> GetPreviousMonthOrderCount(DateTime selectedMonth)
        {
            var previousMonth = selectedMonth.AddMonths(-1);
            var startOfPreviousMonth = new DateTime(previousMonth.Year, previousMonth.Month, 1);
            var endOfPreviousMonth = startOfPreviousMonth.AddMonths(1).AddDays(-1);

            var orderCount = await _dbContext.Order
                .Where(order => order.CreateDate >= startOfPreviousMonth && order.CreateDate <= endOfPreviousMonth)
                .CountAsync();

            return orderCount;
        }
        public async Task<List<Order>> GetBankPaymentOrders(DateTime selectedMonth)
        {
            var bankPaymentOrders = await _dbContext.Order
                .Where(order => order.PaymentMethod == PaymentMethod.ChuyenKhoanNganHang
                             && order.CreateDate.Year == selectedMonth.Year
                             && order.CreateDate.Month == selectedMonth.Month)
                .ToListAsync();

            return bankPaymentOrders;
        }
        [HttpGet]
        [Route("")]
        [Route("indexhome")]
        public async Task<IActionResult> Index(string month)
        {
            DateTime selectedMonth = string.IsNullOrEmpty(month) ? DateTime.Now : DateTime.Parse(month);
            var statistics = await CalculateStatistics(month);
            var previousMonthOrderCount = await GetPreviousMonthOrderCount(selectedMonth);
            ViewBag.PreviousMonthOrderCount = previousMonthOrderCount;
            return View(statistics);
        }
        private XLWorkbook CreateExcelWorkbook(MonthlyStatistic statistics)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Monthly Statistics");

            // Header
            worksheet.Cell("A1").Value = "Tháng";
            worksheet.Cell("B1").Value = "Tổng đơn hàng";
            worksheet.Cell("C1").Value = "Đơn chưa hoàn thành";
            worksheet.Cell("D1").Value = "Đơn hoàn thành";
            worksheet.Cell("E1").Value = "Tổng doanh thu";
            worksheet.Cell("F1").Value = "Tổng chi phí";

            // Data
            worksheet.Cell("A2").Value = statistics.Month.ToString("MM/yyyy");
            worksheet.Cell("B2").Value = statistics.TotalOrder;
            worksheet.Cell("C2").Value = statistics.TotalOrdersnosuccess;
            worksheet.Cell("D2").Value = statistics.TotalOrderssuccess;
            worksheet.Cell("E2").Value = statistics.TotalRevenue;
            worksheet.Cell("F2").Value = statistics.TotalCost;
            //
            worksheet.Cell("E2").Value = statistics.TotalRevenue;
            worksheet.Cell("E2").Style.NumberFormat.Format = "#,##0\\ \"₫\""; // Định dạng VND

            worksheet.Cell("F2").Value = statistics.TotalCost;
            worksheet.Cell("F2").Style.NumberFormat.Format = "#,##0\\ \"₫\""; // Định dạng VND

            return workbook;
        }
        [HttpGet]
        [Route("exporttoexcel")]
        public async Task<IActionResult> ExportToExcel(string month)
        {
            var statistics = await CalculateStatistics(month);
            var workbook = CreateExcelWorkbook(statistics);

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Statistics_{statistics.Month.ToString("yyyy_MM")}.xlsx");
            }
        }
        private async Task<Dictionary<Guid, int>> CalculateBestSellingProducts(DateTime startDate, DateTime endDate)
        {
            var orders = await _dbContext.Order
                .Where(order => order.CreateDate >= startDate && order.CreateDate <= endDate && order.OrderStatus == OrderStatus.Delivered)
                .Include(order => order.OrderVariant)
                .ToListAsync();

            var productSalesCount = new Dictionary<Guid, int>();

            foreach (var order in orders)
            {
                foreach (var orderVariant in order.OrderVariant)
                {
                    var productId = orderVariant.IDOptions;
                    var quantitySold = orderVariant.Quantity;

                    if (!productSalesCount.ContainsKey(productId))
                    {
                        productSalesCount[productId] = quantitySold;
                    }
                    else
                    {
                        productSalesCount[productId] += quantitySold;
                    }
                }
            }

            var sortedProducts = productSalesCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return sortedProducts;
        }
    }
}
