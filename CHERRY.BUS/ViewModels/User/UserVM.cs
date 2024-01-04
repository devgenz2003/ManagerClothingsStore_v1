
namespace CHERRY.BUS.ViewModels.User
{
    public class UserVM
    {
        public string ID { get; set; }
        public string Role_Name { get; set; }
        public string MemberName { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstAndLastName
        {
            get
            {
                return SurName + " " + MiddleName + " " + FirstName;
            }
        }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? SurName { get; set; }
        public string? Gmail { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? ImagePath { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; } // 0 = Delete
    }
}
