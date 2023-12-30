using Microsoft.AspNetCore.Http;

namespace CHERRY.BUS.ViewModels.Helpers
{
    public interface IFileValidator
    {
        bool IsValid(IFormFile file);
    }
}
