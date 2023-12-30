
namespace CHERRY.BUS.ViewModels.User
{
    public class UserUpdateVM
    {
        public Guid ModifieBy { get; set; }
        public Guid? ID_MemberRank { get; set; }
        public string Password { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? SurName { get; set; }
        public string? Gmail { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? ImagePath { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; }
    }
}
