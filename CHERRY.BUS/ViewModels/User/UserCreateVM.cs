using Microsoft.AspNetCore.Http;

namespace CHERRY.BUS.ViewModels.User
{
    public class UserCreateVM
    {
        public string CreateBy { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? SurName { get; set; }
        public string? Gmail { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public IFormFile? ImagePath { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; }
    }
}
