
namespace CHERRY.BUS.ViewModels
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
