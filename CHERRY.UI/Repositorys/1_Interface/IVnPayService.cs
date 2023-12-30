using CHERRY.BUS.ViewModels.Order;
using CHERRY.BUS.ViewModels.Payment;
public interface IVnPayService
{
    public string CreatePaymentUrl(OrderCreateVM model, HttpContext context);
    public Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections);
}
